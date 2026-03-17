using Flowdesks.Application.Responses.Identity;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Identity
{
    public class RolePermissionPagingRequest : PagedRequest, IRequest<Result<PaginatedResult<RolePermissionResponse>>>
    {
        public Guid? RoleId { get; set; }
    }
}
