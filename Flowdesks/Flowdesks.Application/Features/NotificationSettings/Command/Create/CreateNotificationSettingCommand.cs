using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.NotificationSettings;
using Flowdesks.Domain.Entities.Notifications;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.NotificationSettings.Command.Create
{
    public class CreateNotificationSettingCommand : IRequestHandler<CreateUpdateNotificationSettingRequest, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateNotificationSettingCommand(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(CreateUpdateNotificationSettingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var notificationSetting = _mapper.Map<NotificationSetting>(request);

                _unitOfWork.Repository<NotificationSetting>().Add(notificationSetting);
                await _unitOfWork.SaveAsync(cancellationToken);

                return Result<int>.Success();
            }
            catch (Exception ex)
            {
                return Result<int>.Fail(ex.Message);
            }
        }
    }
}
