using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.NotificationSettings;
using Flowdesks.Application.Responses.NotificationSetting;
using Flowdesks.Domain.Entities.Notifications;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.NotificationSettings.Query.GetById
{
    public class GetNotificationSettingByIdQuery : IRequestHandler<GetNotificationSettingByIdRequest, Result<NotificationSettingResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetNotificationSettingByIdQuery(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<NotificationSettingResponse>> Handle(GetNotificationSettingByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var notificationSetting = await _unitOfWork.Repository<NotificationSetting>().Entities().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

                var NotificationSettingResponse = _mapper.Map<NotificationSettingResponse>(notificationSetting);

                return Result<NotificationSettingResponse>.Success(NotificationSettingResponse);
            }
            catch (Exception ex)
            {
                return Result<NotificationSettingResponse>.Fail(ex.Message);
            }
        }
    }
}

