using Flowdesks.Application.Models.Notification;
using Flowdesks.Shared.Wrapper;

namespace Flowdesks.Application.Responses.Notification;

public class NotificationPagedResponse
{
    public long TotalCount { get; set; }
    public long UnreadCount { get; set; }
    public PaginatedResult<NotificationDto> List { get; set; }
}