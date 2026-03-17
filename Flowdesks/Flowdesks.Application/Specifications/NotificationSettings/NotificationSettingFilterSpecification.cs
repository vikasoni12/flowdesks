using Flowdesks.Application.Requests.NotificationSettings;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.Notifications;

namespace Flowdesks.Application.Specifications.NotificationSettings
{
    public class NotificationSettingFilterSpecification : Specification<NotificationSetting>
    {
        public NotificationSettingFilterSpecification(NotificationSettingPagingRequest request)
        {
            if (request.UserId != null)
            {
                And(p => p.UserId.Equals(request.UserId));
            }
        }
    }
}
