using Flowdesks.Application.Interfaces.Notification;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.Notifications.Command.Add;

public class MarkNotificationAsReadCommand : IRequest<Result<string>>
{
    public Guid? NotificationId { get; set; }
}

public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand, Result<string>>
{
    private readonly INotificationService _notificationService;

    public MarkNotificationAsReadCommandHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task<Result<string>> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
    {
        return await _notificationService.MarkAsReadAsync(request.NotificationId);
    }
}