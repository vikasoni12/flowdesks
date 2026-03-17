using Flowdesks.Shared.Notification;

namespace Flowdesks.Application.Requests.Notification;

public class CreateUpdateNotificationRequest
{
    public string Content { get; set; }
    public string? Message { get; set; }
    public string LinkToNotification { get; set; }
    public string EntityId { get; set; }
    public NotificationType NotificationType { get; set; }
}