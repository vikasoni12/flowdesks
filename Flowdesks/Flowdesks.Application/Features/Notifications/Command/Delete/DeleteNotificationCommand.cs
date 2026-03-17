using Flowdesks.Application.Interfaces.Notification;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.Notifications.Command.Delete;

public class DeleteNotificationCommand : IRequest<Result<string>>
{
    public Guid Id { get; set; }
}

public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, Result<string>>
{
    private readonly INotificationService _notificationService;

    public DeleteNotificationCommandHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task<Result<string>> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        return await _notificationService.Delete(request.Id);
    }
}