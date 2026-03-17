using AutoMapper;
using Flowdesks.Application.Extensions;
using Flowdesks.Application.Interfaces.Email;
using Flowdesks.Application.Interfaces.Email.IEmailPopulate;
using Flowdesks.Application.Interfaces.Notification;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Models.Email;
using Flowdesks.Application.Requests.Notification;
using Flowdesks.Application.Requests.Quotes.QuoteResponses;
using Flowdesks.Application.Responses.Quotes;
using Flowdesks.Domain.Entities.Quotes;
using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Notification;
using Flowdesks.Shared.Wrapper;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.TechnicianQuotes.Command.Update
{
    public class UpdateTechnicianQuoteCommand : IRequestHandler<UpdateTechnicianQuoteRequest, Result<TechnicianQuoteResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailPopulateBody _emailPopulateBody;
        private readonly IEmailSender _mailService;
        private readonly INotificationService _notificationService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateTechnicianQuoteCommand(IUnitOfWork unitOfWork, IMapper mapper, IEmailPopulateBody emailPopulateBody, IEmailSender mailService, INotificationService notificationService, ICurrentUserService currentUserService, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailPopulateBody = emailPopulateBody;
            _mailService = mailService;
            _notificationService = notificationService;
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<TechnicianQuoteResponse>> Handle(UpdateTechnicianQuoteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var technicianQuote = await _unitOfWork.Repository<TechnicianQuote>().Entities()
                    .Include(x => x.Technician)
                    .Include(x => x.Supplier)
                    .Include(x => x.Quote)
                    .FirstOrDefaultAsync(x => x.Id.Equals(request.Id));

                if (technicianQuote != null)
                {
                    string existingQuoteStatusMessage = await CheckOrUpdateQuoteStatus(request, technicianQuote);

                    await UpdateTechnicianQuote(request, technicianQuote);

                    await _unitOfWork.SaveAsync(cancellationToken);

                    if (request.IsNewResponse)
                    {
                        await CreateNotification("New response received on quote", $"Response received from {technicianQuote.Technician?.Name} on Quote - {technicianQuote.Quote?.QuoteNumber}",
               technicianQuote.Quote?.QuoteNumber, technicianQuote.CreatedBy);
                    }

                    BackgroundJob.Enqueue(() => SendStatusEmail(technicianQuote.Id));

                    var response = _mapper.Map<TechnicianQuoteResponse>(technicianQuote);
                    response.AlertMessage = existingQuoteStatusMessage;

                    return Result<TechnicianQuoteResponse>.Success(response);
                }
                else
                {
                    return Result<TechnicianQuoteResponse>.Fail($"technicianQuote with {request.Id} not found");
                }

            }
            catch (Exception ex)
            {
                return Result<TechnicianQuoteResponse>.Fail(ex.Message);
            }
        }

        private async Task UpdateTechnicianQuote(UpdateTechnicianQuoteRequest request, TechnicianQuote technicianQuote)
        {
            if (request.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase) && technicianQuote.Status.Equals("Requested", StringComparison.OrdinalIgnoreCase))
            {
                _mapper.Map(request, technicianQuote);
            }
            else if (technicianQuote.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase) && !request.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase))
            {
                request.Cost = technicianQuote.Cost.Value;
                request.Response = technicianQuote.Response;
                request.ProposedJobDate = technicianQuote.ProposedJobDate;
                _mapper.Map(request, technicianQuote);
                technicianQuote.ApproverId = new Guid(_currentUserService.UserId);
            
            }
            _unitOfWork.Repository<TechnicianQuote>().Update(technicianQuote);
        }

        private async Task<string> CheckOrUpdateQuoteStatus(UpdateTechnicianQuoteRequest request, TechnicianQuote technicianQuote)
        {
            var message = string.Empty;
            var quote = await _unitOfWork.Repository<Domain.Entities.Quotes.Quote>().GetByIdAsync(request.QuoteId);

            if (request.Status.Equals(QuoteStatus.Accepted.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                if (quote != null && (technicianQuote.ApproverId == null || technicianQuote.ApproverId == Guid.Empty))
                {
                    quote.Status = QuoteStatus.Accepted.ToString();
                    _unitOfWork.Repository<Domain.Entities.Quotes.Quote>().Update(quote);
                }
                else if (quote != null && technicianQuote.ApproverId != null)
                {
                    message = $"Quote has already been marked as {quote.Status}";
                }
            }
            else if (request.Status.Equals(QuoteStatus.Declined.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                if (quote != null && (technicianQuote.ApproverId == null || technicianQuote.ApproverId == Guid.Empty))
                {
                    quote.Status = QuoteStatus.Declined.ToString();
                    _unitOfWork.Repository<Quote>().Update(quote);
                }
                else if (quote != null && technicianQuote.ApproverId != null)
                {
                    message = $"Quote has already been marked as {quote.Status}";
                }
            }
            return message;
        }

        private async Task CreateNotification(string content, string message, string quoteId, string entityId)
        {
            await _notificationService.Create(new CreateUpdateNotificationRequest
            {
                Content = content,
                Message = message,
                LinkToNotification = $"/quotes/{quoteId}",
                EntityId = entityId,
                NotificationType = NotificationType.Quote
            });
        }

        public async Task SendStatusEmail(Guid id)
        {
            var technicianQuote = await _unitOfWork.Repository<TechnicianQuote>().Entities()
                .Include(x => x.Technician)
                .Include(x => x.Supplier)
                .Include(x => x.Quote)
                .ThenInclude(x => x.Building)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            string recipientEmail = technicianQuote?.Technician != null ? technicianQuote?.Technician.Email : technicianQuote.Supplier.Email;
            if (technicianQuote == null || recipientEmail == null)
                return;

            if (technicianQuote != null)
            {
                var httpContext = _httpContextAccessor.HttpContext;
                technicianQuote.ConvertUtcDateTimePropertiesToLocalTime(httpContext);

                if (technicianQuote.Status.Equals(QuoteStatus.Accepted.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    string templateName = "QuoteRequestApprovalTemplate.html";
                    string body = await _emailPopulateBody.PopulateBody(templateName);

                    body = body.Replace("{userName}", technicianQuote.Technician != null ? technicianQuote.Technician.Name: technicianQuote.Supplier.Name);
                    body = body.Replace("{QuoteNumber}", technicianQuote.Quote.QuoteNumber)
                               .Replace("{JobType}", technicianQuote.Quote.JobType)
                               .Replace("{Building}", technicianQuote.Quote.Building.Name)
                               .Replace("{ProposedJobDate}", technicianQuote.ProposedJobDate.Value.ToShortDateString())
                               .Replace("{Cost}", technicianQuote.Cost.ToString());

                    string subject = "Quote Request Approved";

                    _mailService.SendEmail(new EmailMessage
                    {
                        To = recipientEmail,
                        Body = body,
                        Subject = subject
                    });
                }
                else if (technicianQuote.Status.Equals(QuoteStatus.Declined.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    string templateName = "QuoteRequestDeclineTemplate.html";
                    string body = await _emailPopulateBody.PopulateBody(templateName);

                    body = body.Replace("{userName}", technicianQuote.Technician.Name);
                    body = body.Replace("{quoteNumber}", technicianQuote.Quote.QuoteNumber);

                    string subject = "Quote Request Declined";

                    _mailService.SendEmail(new EmailMessage
                    {
                        To = recipientEmail,
                        Body = body,
                        Subject = subject
                    });
                }
            }
        }
    }
}