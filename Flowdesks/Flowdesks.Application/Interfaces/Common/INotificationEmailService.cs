namespace Flowdesks.Application.Interfaces.Common;

public interface INotificationEmailService
{
    Task SendNotification();
    Task SendEmail();
}