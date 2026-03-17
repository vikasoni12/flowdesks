using Flowdesks.Application.Requests;
using Flowdesks.Application.Specifications.Base;

namespace Flowdesks.Application.Specifications.Notification;

public class NotificationFilterSpecification : Specification<Domain.Entities.Notification.Notification>
{
    public NotificationFilterSpecification(PagedRequest request)
    {
        if (!string.IsNullOrEmpty(request.StringSearch))
        {
            And(user =>
                    user.Content.ToLower().Contains(request.StringSearch) ||
                    user.Message.ToLower().Contains(request.StringSearch));
        }
    }
}