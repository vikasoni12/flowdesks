using Flowdesks.Application.Responses.Chat.GroupMessage;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Chat.GroupMessage
{
    public class GroupMessagePagingRequest : PagedRequest, IRequest<Result<PaginatedResult<GroupMessageResponse>>>
    {
        public Guid GroupId { get; set; }
    }
}
