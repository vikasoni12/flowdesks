using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.NotificationSettings;
using Flowdesks.Domain.Entities.Notifications;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.NotificationSettings.Command.Update
{
    public class UpdateNotificationSettingCommand : CreateUpdateNotificationSettingRequest, IRequest<Result<int>>
    {
        public Guid Id { get; set; }
    }
    public class UpdateNotificationSettingCommandHandler : IRequestHandler<UpdateNotificationSettingCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateNotificationSettingCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(UpdateNotificationSettingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var notificationSetting = await _unitOfWork.Repository<NotificationSetting>().GetByIdAsync(request.Id);

                if (notificationSetting != null)
                {

                    _mapper.Map(request, notificationSetting);

                    _unitOfWork.Repository<NotificationSetting>().Update(notificationSetting);

                    await _unitOfWork.SaveAsync(cancellationToken);

                    return Result<int>.Success("Notification Setting updated successfully");
                }
                else
                {
                    return Result<int>.Fail($"Notification Setting with {request.Id} not found");
                }

            }
            catch (Exception ex)
            {
                return Result<int>.Fail(ex.Message);
            }
        }
    }
}
