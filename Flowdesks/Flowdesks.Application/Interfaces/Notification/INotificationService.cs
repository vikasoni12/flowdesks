using Flowdesks.Application.Models.Notification;
using Flowdesks.Application.Requests;
using Flowdesks.Application.Requests.Notification;
using Flowdesks.Application.Responses.Notification;
using Flowdesks.Shared.Notification;
using Flowdesks.Shared.Wrapper;

namespace Flowdesks.Application.Interfaces.Notification;

public interface INotificationService
{
    Task<Result<NotificationDto>> GetByIdAsync(Guid notificationId);
    Task<Result<NotificationDto>> GetByEntityIdAsync(string entityId);
    Task<Result<NotificationPagedResponse>> GetListAsync(PagedRequest request);
    Task<Result<string>> Create(CreateUpdateNotificationRequest notification);
    Task<Result<string>> Delete(Guid notificationId);
    Task<Result<string>> Delete(Guid entityId, NotificationType type);
    Task<Result<string>> MarkAsReadAsync(Guid? notificationId);
}