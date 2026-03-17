using Flowdesks.Application.Features.Buildings.Messagess.Query;
using Flowdesks.Application.Features.Chats.DirectChat.Command;
using Flowdesks.Application.Requests.Chat.DirectMessage;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.Chat;

namespace Flowdesks.Application.Specifications.Chat;

public class DirectMessageFilterSpecification : Specification<DirectMessage>
{
    public DirectMessageFilterSpecification(DirectMessagePagingRequest request)
    {
        if (request.SenderId != Guid.Empty && request.ReceiverId != Guid.Empty)
        {
            And(p => (p.SenderId.Equals(request.SenderId) && p.ReceiverId.Equals(request.ReceiverId))
            || (p.ReceiverId.Equals(request.SenderId) && p.SenderId.Equals(request.ReceiverId)));

            And(p => p.DeletedBy != request.SenderId.ToString());
        }

        if (!string.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.Message.Content.Contains(request.StringSearch));
        }
    }

    public DirectMessageFilterSpecification(GetMessagesByUserIdQuery request)
    {
        if (request.UserId != Guid.Empty)
        {
            And(p => (p.ReceiverId.Equals(request.UserId) || p.SenderId.Equals(request.UserId))
            && p.DeletedBy != request.UserId.ToString());
        }
    }

    public DirectMessageFilterSpecification(DeleteAllDirectMessageCommand request, string currentUserId)
    {
        if (request.UserId != Guid.Empty)
        {
            And(p => (p.SenderId.Equals(request.UserId) && p.ReceiverId.ToString().Equals(currentUserId))
            || (p.ReceiverId.Equals(request.UserId) && p.SenderId.ToString().Equals(currentUserId)));

            And(p => p.DeletedBy != currentUserId);
        }
    }
}
