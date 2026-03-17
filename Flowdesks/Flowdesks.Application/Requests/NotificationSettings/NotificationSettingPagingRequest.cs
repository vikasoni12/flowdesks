using Flowdesks.Application.Responses.NotificationSetting;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.NotificationSettings
{
    public class NotificationSettingPagingRequest : PagedRequest, IRequest<Result<PaginatedResult<NotificationSettingResponse>>>
    {   
        public Guid? UserId { get; set; }
    }
}
