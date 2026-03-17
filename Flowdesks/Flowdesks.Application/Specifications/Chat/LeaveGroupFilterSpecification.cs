using Flowdesks.Application.Features.Chats.Groups.Query;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.Chat;

namespace Flowdesks.Application.Specifications.Chat
{
    public class LeaveGroupFilterSpecification : Specification<GroupUser>
    {
        public LeaveGroupFilterSpecification(LeaveGroupQuery request)
        {
            if (request.UserId != Guid.Empty)
            {
                And(p => p.UserId.Equals(request.UserId));
            }

            if (request.GroupId != Guid.Empty)
            {
                And(p => p.GroupId.Equals(request.GroupId));
            }
        }
    }
}
