using Flowdesks.Application.Responses.Chat.Groups;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Chat.Group
{
    public class GroupPagingRequest : PagedRequest, IRequest<Result<PaginatedResult<GroupResponse>>>
    {
        public Guid UserId { get; set; }
    }
}
