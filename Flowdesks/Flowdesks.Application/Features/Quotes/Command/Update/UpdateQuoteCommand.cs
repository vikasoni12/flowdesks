using AutoMapper;
using Flowdesks.Application.Interfaces.Email.IEmailPopulate;
using Flowdesks.Application.Interfaces.Email;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.Quotes;
using Flowdesks.Application.Responses.Quotes;
using Flowdesks.Domain.Entities.Quotes;
using Flowdesks.Domain.Entities.Technicians;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Hangfire;
using Flowdesks.Application.Models.Email;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Http;
using Flowdesks.Application.Extensions;
using Flowdesks.Application.Interfaces.Identity;
using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Domain.Entities.Sites;
using Flowdesks.Application.Requests.UploadFiles;
using static Flowdesks.Shared.Constants.Common.ApplicationConstants;
using static Flowdesks.Shared.Constants.Api.ApiConstants;
using Flowdesks.Domain.Entities.Notifications;
using System.Linq;
using Flowdesks.Application.Interfaces.Notification;
using Flowdesks.Application.Requests.Notification;
using Flowdesks.Shared.Notification;

namespace Flowdesks.Application.Features.Quotes.Command.Update
{
    public class UpdateQuoteCommand : IRequestHandler<UpdateQuoteRequest, Result<QuoteResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IUploadService _uploadService;
        private readonly IEmailPopulateBody _emailPopulateBody;
        private readonly IEmailSender _mailService;
        private readonly INotificationService _notificationService;

        public UpdateQuoteCommand(IUnitOfWork unitOfWork, IMapper mapper, IEmailSender mailService, IEmailPopulateBody emailPopulateBody, ICurrentUserService currentUserService, IHttpContextAccessor httpContextAccessor, IUserService userService, IUploadService uploadService, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mailService = mailService;
            _emailPopulateBody = emailPopulateBody;
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _uploadService = uploadService;
            _notificationService=notificationService;
        }

        public async Task<Result<QuoteResponse>> Handle(UpdateQuoteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var quote = await _unitOfWork.Repository<Quote>().GetByIdAsync(request.Id);

                if (quote != null)
                {
                    if (!string.IsNullOrEmpty(quote.ProfilePictureUrl) && (string.IsNullOrEmpty(request.ProfilePictureUrl) || request.ImageName?.FileName != null))
                    {
                        await _uploadService.DeleteAsync(quote.ProfilePictureUrl);
                    }

                    _mapper.Map(request, quote);

                    if (request.ImageName != null && request.ImageName.FileName != null)
                    {
                        var profilePictureUrl = await _uploadService.UploadAsync(new UploadRequest
                        {
                            Path = FileUploadUrl.Quote,
                            FileBytes = request.ImageName.Content,
                            FileName = request.ImageName.FileName
                        });

                        quote.ProfilePictureUrl = profilePictureUrl.Data;
                    }

                    _unitOfWork.Repository<Quote>().Update(quote);

                    if (request.SendToIds?.Count > 0)
                    {
                        var addedTechnicians = await AddRelatedEntities(quote, request);

                        await _unitOfWork.SaveAsync(cancellationToken);

                        if (addedTechnicians.Any())
                        {
                            BackgroundJob.Enqueue(() => SendTechniciansEmail(quote.Id, addedTechnicians, _currentUserService.OriginUrl));
                        }
                    }
                    else
                    {
                        await _unitOfWork.SaveAsync(cancellationToken);
                    }

                    var userIds = request.SendToIds
                        .Select(sendTo => sendTo.TechnicianId ?? sendTo.SupplierId)
                        .Where(id => id != null).Distinct();

                    var notificationSettings = await _unitOfWork.Repository<NotificationSetting>().Entities()
                        .Where(x => userIds.Contains(x.UserId) && x.IsQuoteReceived == true).ToListAsync(cancellationToken);

                    if(notificationSettings.Any())
                    {
                        foreach (var userNotificationSetting in notificationSettings)
                        {
                            await CreateNotification(
                                        "Quote updated",
                                        "Please review the updated quote and provide your response accordingly.",
                                        quote.QuoteNumber.ToString(),
                                        userNotificationSetting.UserId.ToString());
                        }
                    }


                    var quoteResponse = _mapper.Map<QuoteResponse>(quote);

                    return Result<QuoteResponse>.Success(quoteResponse);
                }
                else
                {
                    return Result<QuoteResponse>.Fail($"Quote with {request.Id} not found");
                }

            }
            catch (Exception ex)
            {
                return Result<QuoteResponse>.Fail(ex.Message);
            }
        }

        private async Task<IEnumerable<TechnicianQuote>> AddRelatedEntities(Quote quote, UpdateQuoteRequest request)
        {
            var exitingTechnicians = _unitOfWork.Repository<TechnicianQuote>().Entities().Where(x => x.QuoteId == quote.Id).ToList();

            var quoteToRemove = exitingTechnicians.Where(tech => !request.SendToIds.Any(x => x.TechnicianId == tech.TechnicianId || x.SupplierId == tech.SupplierId)).ToList();

            var quoteToAdd = request.SendToIds.Where(x => !exitingTechnicians.Any(e => e.TechnicianId == x.TechnicianId || e.SupplierId == x.SupplierId)).Select(
                technician => new TechnicianQuote { QuoteId = quote.Id, TechnicianId = technician.TechnicianId, SupplierId = technician.SupplierId, Cost = request.Cost, Status = request.Status });

            if (quoteToRemove.Any())
            {
                _unitOfWork.Repository<TechnicianQuote>().DeleteRange(quoteToRemove);
            }

            if (quoteToAdd.Any())
            {
                _unitOfWork.Repository<TechnicianQuote>().AddRange(quoteToAdd);
            }

            return quoteToAdd;
        }

        public async Task SendTechniciansEmail(Guid quoteId, IEnumerable<TechnicianQuote> addedTechnicians, string url)
        {
            var quote = await _unitOfWork.Repository<Quote>().Entities().Include(x => x.TechnicianQuotes)
                .ThenInclude(x => x.Technician)
                .Include(x => x.Building)
                .FirstOrDefaultAsync(x => x.Id.Equals(quoteId));

            if (quote == null) { return; }

            var httpContext = _httpContextAccessor.HttpContext;
            quote.ConvertUtcDateTimePropertiesToLocalTime(httpContext);

            string route = "#/quote-response-form";

            var endpointUri = !string.IsNullOrEmpty(url) ? new Uri(string.Concat($"{url}/", route)) : new Uri(route);
            string templateName = "TechnicianQuoteTemplate.html";

            var technicianQuotes = quote.TechnicianQuotes
                .Where(x => addedTechnicians.Any(y => y.TechnicianId.Equals(x.TechnicianId)));

            foreach (var technicianQuote in technicianQuotes)
            {
                string body = await _emailPopulateBody.PopulateBody(templateName);
                string token = _userService.GenerateToken(technicianQuote.Technician?.Email ?? technicianQuote.Supplier?.Email, technicianQuote.TenantId);
                var link = $"{endpointUri}/{EncodeIds(quote.QuoteNumber, technicianQuote.Id)}?token={token}";

                body = body.Replace("{userName}", technicianQuote.Technician!= null?technicianQuote.Technician.Name: technicianQuote.Supplier.Name);
                body = body.Replace("{link}", link);
                body = body.Replace("{QuoteNumber}", quote.QuoteNumber)
                           .Replace("{JobType}", quote.JobType)
                           .Replace("{Building}", quote.Building.Name)
                           .Replace("{JobDate}", quote.JobDate.Value.ToShortDateString())
                           .Replace("{Cost}", quote.Cost.ToString());

                string subject = "New Quote Assignment";
                string recipientEmail = technicianQuote.Technician!= null ? technicianQuote.Technician.Email : technicianQuote.Supplier.Email;

                if (recipientEmail == null)
                    return;

                _mailService.SendEmail(new EmailMessage
                {
                    To = recipientEmail,
                    Body = body,
                    Subject = subject
                });
            }
        }

        public static string EncodeIds(string quoteNumber, Guid technicianId)
        {
            string combinedIds = $"{quoteNumber}|{technicianId}";
            byte[] bytes = Encoding.UTF8.GetBytes(combinedIds);
            string encodedString = Convert.ToBase64String(bytes);
            string urlEncodedString = Uri.EscapeDataString(encodedString);

            return urlEncodedString;
        }

        private async Task CreateNotification(string content, string message, string quoteNumber, string entityId)
        {
            await _notificationService.Create(new CreateUpdateNotificationRequest
            {
                Content = content,
                Message = message,
                LinkToNotification = $"/quotes/{quoteNumber}",
                EntityId = entityId,
                NotificationType = NotificationType.Quote
            });
        }
    }
}