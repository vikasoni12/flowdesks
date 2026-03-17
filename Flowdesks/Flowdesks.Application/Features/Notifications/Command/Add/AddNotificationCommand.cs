using Flowdesks.Application.Interfaces.Notification;
using Flowdesks.Application.Requests.Notification;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.Notifications.Command.Add;

public class AddNotificationCommand : CreateUpdateNotificationRequest, IRequest<Result<string>>
{
}

public class AddNotificationCommandHandler : IRequestHandler<AddNotificationCommand, Result<string>>
{
    private readonly INotificationService _notificationService;

    public AddNotificationCommandHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task<Result<string>> Handle(AddNotificationCommand request, CancellationToken cancellationToken)
    {
        return await _notificationService.Create(request);
    }
}