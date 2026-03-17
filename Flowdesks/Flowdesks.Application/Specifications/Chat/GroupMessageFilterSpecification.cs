using Flowdesks.Application.Requests.Chat.GroupMessage;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.Chat;

namespace Flowdesks.Application.Specifications.Chat
{
    public class GroupMessageFilterSpecification : Specification<GroupMessage>
    {
        public GroupMessageFilterSpecification(GroupMessagePagingRequest request, string currentUserId)
        {
            if (request.GroupId != Guid.Empty)
            {
                And(p => p.GroupId.Equals(request.GroupId));
            }

            if (!string.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.Message.Content.Contains(request.StringSearch));
            }
        }
    }
}
