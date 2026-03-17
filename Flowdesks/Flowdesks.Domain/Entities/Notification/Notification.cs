using Flowdesks.Domain.Common;

namespace Flowdesks.Domain.Entities.Notification;

public class Notification : AuditableEntity<Guid>
{
    public string Content { get; set; }
    public string? Message { get; set; }
    public string LinkToNotification { get; set; }
    public string EntityId { get; set; }
    public string NotificationType { get; set; }
    public bool IsStatusChanged { get; set; } = false;
    public DateTime? ReadOn { get; set; }
    public bool IsRead { get; set; } = false;
}