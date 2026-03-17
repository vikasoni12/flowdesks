using Flowdesks.Application.Interfaces.Notification;
using Flowdesks.Application.Models.Notification;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.Notifications.Query.GetById;

public class GetNotificationByIdQuery : IRequest<Result<NotificationDto>>
{
    public Guid Id { get; set; }
}

public class GetNotificationByIdQueryHandler : IRequestHandler<GetNotificationByIdQuery, Result<NotificationDto>>
{
    private readonly INotificationService _notificationService;

    public GetNotificationByIdQueryHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task<Result<NotificationDto>> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
    {
        return await _notificationService.GetByIdAsync(request.Id);
    }
}