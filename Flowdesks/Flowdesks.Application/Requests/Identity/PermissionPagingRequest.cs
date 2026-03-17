using Flowdesks.Application.Responses.Identity;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Identity
{
    public class PermissionPagingRequest : PagedRequest, IRequest<Result<PaginatedResult<PermissionsResponse>>>
    {
    }
}
