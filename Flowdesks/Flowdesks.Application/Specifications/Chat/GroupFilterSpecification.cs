using Flowdesks.Application.Requests.Chat.Group;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.Chat;

namespace Flowdesks.Application.Specifications.Chat
{
    public class GroupFilterSpecification : Specification<Group>
    {
        public GroupFilterSpecification(GroupPagingRequest request)
        {
            if (request.UserId != Guid.Empty)
            {
                And(p => p.CreatedBy.Equals(request.UserId.ToString()) || p.GroupUsers.Any(x => x.UserId.Equals(request.UserId)));
            }

            if (!string.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.GroupName.Contains(request.StringSearch));
            }
        }
    }
}
