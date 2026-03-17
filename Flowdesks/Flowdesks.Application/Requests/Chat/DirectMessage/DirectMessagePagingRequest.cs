using Flowdesks.Application.Responses.Chat.DirectMessage;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Chat.DirectMessage
{
    public class DirectMessagePagingRequest : PagedRequest, IRequest<Result<PaginatedResult<DirectMessageResponse>>>
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
    }
}
