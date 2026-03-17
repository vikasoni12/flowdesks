using Flowdesks.Application.Responses.NotificationSetting;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.NotificationSettings
{
    public class GetNotificationSettingByIdRequest : IRequest<Result<NotificationSettingResponse>>
    {
        public Guid Id { get; set; }
    }
}
