using AutoMapper;
using Flowdesks.Application.Extensions;
using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.Email;
using Flowdesks.Application.Interfaces.Email.IEmailPopulate;
using Flowdesks.Application.Interfaces.Identity;
using Flowdesks.Application.Interfaces.Notification;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Models.Email;
using Flowdesks.Application.Requests.Notification;
using Flowdesks.Application.Requests.Quotes;
using Flowdesks.Application.Requests.UploadFiles;
using Flowdesks.Domain.Entities.Notifications;
using Flowdesks.Domain.Entities.Quotes;
using Flowdesks.Shared.Notification;
using Flowdesks.Shared.Wrapper;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text;
using static Flowdesks.Shared.Constants.Common.ApplicationConstants;

namespace Flowdesks.Application.Features.Quotes.Command.Create
{
    public class CreateQuoteCommand : IRequestHandler<CreateQuoteRequest, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEmailPopulateBody _emailPopulateBody;
        private readonly IEmailSender _mailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IUploadService _uploadService;
        private readonly INotificationService _notificationService;

        public CreateQuoteCommand(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, IEmailPopulateBody emailPopulateBody, IEmailSender mailService, IHttpContextAccessor httpContextAccessor, IUserService userService, IUploadService uploadService, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _emailPopulateBody = emailPopulateBody;
            _mailService = mailService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _uploadService = uploadService;
            _notificationService=notificationService;
        }

        public async Task<Result<int>> Handle(CreateQuoteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var quote = _mapper.Map<CreateQuoteRequest, Quote>(request);
                quote.QuoteNumber = GenerateQuoteNumber();

                if (request.ProfilePicture?.FileName != null)
                {
                    var profilePictureUrl = await _uploadService.UploadAsync(new UploadRequest
                    {
                        Path = FileUploadUrl.Quote,
                        FileBytes = request.ProfilePicture.Content,
                        FileName = request.ProfilePicture.FileName
                    });

                    quote.ProfilePictureUrl = profilePictureUrl.Data;
                }

                var response = _unitOfWork.Repository<Quote>().Add(quote);
                AddRelatedEntities(quote, request);

                await _unitOfWork.SaveAsync(cancellationToken);

                await SendTechniciansEmail(response.Id, _currentUserService.OriginUrl);

                // Extract all TechnicianIds and SupplierIds from sendToIds
                var userIds = request.SendToIds
                    .Select(sendTo => sendTo.TechnicianId ?? sendTo.SupplierId)
                    .Where(id => id != null).Distinct();

                var notificationSettings = await _unitOfWork.Repository<NotificationSetting>().Entities()
                    .Where(x => userIds.Contains(x.UserId) && x.IsQuoteReceived == true).ToListAsync(cancellationToken);

                if (notificationSettings.Any())
                {
                    foreach (var userNotificationSetting in notificationSettings)
                    {
                        await CreateNotification(
                                    "Quote Created",
                                    "Please review the quote and provide your response accordingly.",
                                    quote.QuoteNumber.ToString(),
                                    userNotificationSetting.UserId.ToString());
                    }
                }

                return Result<int>.Success("Quote saved successfully");
            }
            catch (Exception ex)
            {
                return Result<int>.Fail(ex.Message);
            }
        }

        private string GenerateQuoteNumber()
        {
            // Get the highest existing asset code
            var lastQuote = _unitOfWork.Repository<Quote>().Entities().OrderByDescending(a => a.QuoteNumber).FirstOrDefault();

            // Initialize the new code
            int newCode;

            if (lastQuote != null)
            {
                // Increment the last asset code
                if (int.TryParse(lastQuote.QuoteNumber, out int lastCode))
                {
                    newCode = lastCode + 1;
                }
                else
                {
                    // Handle the case where the last code is not a valid integer
                    newCode = 10001; // or any default starting point
                }
            }
            else
            {
                // If no assets exist, start with a default code
                newCode = 10001; // or any default starting point
            }

            // Check if any items with lower numbers have been deleted
            while (_unitOfWork.Repository<Quote>().Entities().Any(a => a.QuoteNumber == newCode.ToString()))
            {
                // If the code already exists, increment and check again
                newCode++;
            }

            return newCode.ToString();
        }

        private void AddRelatedEntities(Quote quote, CreateQuoteRequest request)
        {
            var technicianQuotes = request.SendToIds
                            .Select(technician => new TechnicianQuote { QuoteId = quote.Id, TechnicianId = technician.TechnicianId,SupplierId= technician.SupplierId, Cost = request.Cost, Status = request.Status.ToString() })
                            .ToList();

            _unitOfWork.Repository<TechnicianQuote>().AddRange(technicianQuotes);
        }

        public async Task SendTechniciansEmail(Guid quoteId, string url)
        {
            var quote = await _unitOfWork.Repository<Quote>().Entities().Include(x => x.TechnicianQuotes)
                .ThenInclude(x => x.Technician)
                .Include(x => x.TechnicianQuotes)
                .ThenInclude(x => x.Supplier)
                .Include(x => x.Building)
                .Include(x => x.Site)
                .FirstOrDefaultAsync(x => x.Id.Equals(quoteId));

            string route = "#/quote-response-form";

            var endpointUri = !string.IsNullOrEmpty(url) ? new Uri(string.Concat($"{url}/", route)) : new Uri(route);
            string templateName = "TechnicianQuoteTemplate.html";

            if (quote != null)
            {
                var httpContext = _httpContextAccessor.HttpContext;
                quote.ConvertUtcDateTimePropertiesToLocalTime(httpContext);

                foreach (var technician in quote?.TechnicianQuotes)
                {
                    string token = _userService.GenerateToken(technician.Technician?.Email ?? technician.Supplier?.Email, technician.TenantId);
                    var link = $"{endpointUri}/{EncodeIds(quote.QuoteNumber, technician.Id)}?token={token}";

                    string body = await _emailPopulateBody.PopulateBody(templateName);
                    body = body.Replace("{userName}", technician.Technician !=null ? technician.Technician.Name: technician.Supplier.Name);
                    body = body.Replace("{link}", link);
                    body = body.Replace("{QuoteNumber}", quote.QuoteNumber)
                               .Replace("{JobType}", quote.JobType)
                               .Replace("{Site}", quote.Site.Name)
                               .Replace("{Building}", quote.Building.Name)
                               .Replace("{JobDate}", quote.JobDate.Value.ToShortDateString())
                               .Replace("{Cost}", quote.Cost.ToString());

                    string subject = "New Quote Assignment";
                    string recipientEmail = technician.Technician !=null ? technician.Technician.Email : technician.Supplier.Email;

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
