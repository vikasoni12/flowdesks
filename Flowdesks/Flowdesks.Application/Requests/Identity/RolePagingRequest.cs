using Flowdesks.Application.Responses.Identity;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Identity
{
    public class RolePagingRequest : PagedRequest, IRequest<Result<PaginatedResult<RoleResponse>>>
    {
        public Guid? UserId { get; set; }
    }
}
