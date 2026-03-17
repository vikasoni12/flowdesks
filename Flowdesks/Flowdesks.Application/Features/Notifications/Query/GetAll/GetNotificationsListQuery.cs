using Flowdesks.Application.Interfaces.Notification;
using Flowdesks.Application.Requests;
using Flowdesks.Application.Responses.Notification;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.Notifications.Query.GetAll;

public class GetNotificationsListQuery : PagedRequest, IRequest<Result<NotificationPagedResponse>>
{
}

public class GetNotificationsListQueryHandler : IRequestHandler<GetNotificationsListQuery, Result<NotificationPagedResponse>>
{
    private readonly INotificationService _notificationService;

    public GetNotificationsListQueryHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task<Result<NotificationPagedResponse>> Handle(GetNotificationsListQuery request, CancellationToken cancellationToken)
    {
        return await _notificationService.GetListAsync(request);
    }
}