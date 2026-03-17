using Flowdesks.Application.Responses.Chat.DirectMessage;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Chat.DirectMessage
{
    public class UserMessageRequest : IRequest<Result<PaginatedResult<UserDirectMessagesResponse>>>
    {
        public Guid UserId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
