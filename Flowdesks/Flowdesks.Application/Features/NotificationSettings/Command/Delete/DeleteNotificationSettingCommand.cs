using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Domain.Entities.Notifications;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.NotificationSettings.Command.Delete
{
    public class DeleteNotificationSettingCommand : IRequest<Result<int>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteNotificationSettingCommandHandler : IRequestHandler<DeleteNotificationSettingCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteNotificationSettingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteNotificationSettingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var notificationSettings = await _unitOfWork.Repository<NotificationSetting>().FirstOrDefaultAsync(x => x.Id == request.Id);

                if (notificationSettings == null)
                {
                    return await Result<int>.FailAsync($"Not found");
                }

                _unitOfWork.Repository<NotificationSetting>().Delete(notificationSettings.Id);
                await _unitOfWork.SaveAsync(cancellationToken);

                return await Result<int>.SuccessAsync("Notification Setting deleted successfully");
            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync(ex.Message);
            }
        }
    }
}
