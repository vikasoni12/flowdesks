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
using Flowdesks.Shared.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;

namespace Flowdesks.Infrastructure.Services.Common;

public class NotificationEmailService : INotificationEmailService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;
    private readonly IEmailSender _mailService;
    private readonly IEmailPopulateBody _emailPopulateBody;
    private readonly ICurrentUserService _currentUserService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public NotificationEmailService(IUnitOfWork unitOfWork, INotificationService notificationService, IEmailSender mailService, IEmailPopulateBody emailPopulateBody, ICurrentUserService currentUserService, IHttpContextAccessor httpContextAccessor, IUserService userService, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
        _mailService = mailService;
        _emailPopulateBody = emailPopulateBody;
        _currentUserService = currentUserService;
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
        _configuration = configuration;
    }

    public async Task SendNotification()
    {
        

       

        
    }

    private static DateTime? CalculateNextNotificationEmailDate(DateTime currentDate, DateTime nextServiceDate, string frequency, int frequencyOccurrence, decimal? period)
    {
        if (frequencyOccurrence <= 0)
        {
            return null;
        }

        var nextNotificationDate = CalculateNextOccurrence(currentDate, nextServiceDate, frequency, frequencyOccurrence);
        if (period == null)
        {
            return nextNotificationDate;
        }
        else
        {
            var occurrences = (currentDate - nextServiceDate).TotalDays / GetFrequencyUnitInDays(frequency) * frequencyOccurrence + 1;

            if ((decimal)occurrences > period)
                return null;

            return nextNotificationDate;
        }
    }

    private static DateTime? CalculateNextOccurrence(DateTime currentDate, DateTime nextServiceDate, string frequency, int frequencyOccurrence)
    {
        switch (frequency.ToUpper())
        {
            case "DAILY":
                var daysDiff = (currentDate - nextServiceDate).TotalDays;
                return nextServiceDate.AddDays(Math.Ceiling(daysDiff / frequencyOccurrence) * frequencyOccurrence);
            case "WEEKLY":
                var weeksDiff = Math.Ceiling((currentDate - nextServiceDate).TotalDays / 7);
                return nextServiceDate.AddDays(Math.Ceiling(weeksDiff / frequencyOccurrence) * 7 * frequencyOccurrence);
            case "MONTHLY":
                var monthDiff = currentDate.Month - nextServiceDate.Month
                                + (currentDate.Year - nextServiceDate.Year) * 12;
                var monthsToAdd = (int)Math.Ceiling((double)monthDiff / frequencyOccurrence) * frequencyOccurrence;
                return new DateTime(nextServiceDate.Year,
                                    nextServiceDate.Month + monthsToAdd,
                                    nextServiceDate.Day);
            case "YEARLY":
                return nextServiceDate.AddYears(frequencyOccurrence);
            default:
                return null;
        }
    }

    private static int GetFrequencyUnitInDays(string frequency)
    {
        return frequency.ToUpper() switch
        {
            "DAILY" => 1,
            "WEEKLY" => 7,
            "MONTHLY" => DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month),
            "YEARLY" => DateTime.IsLeapYear(DateTime.UtcNow.Year) ? 366 : 365,
            _ => 0,
        };
    }

   

    private string GetBaseUrl()
    {
        string environment = _configuration["Environment"];
        string baseUrl = _configuration[$"BaseUrls:{environment}"];

        if (!string.IsNullOrEmpty(baseUrl))
        {
            return baseUrl;
        }
        return null;
    }
}