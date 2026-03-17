using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Requests.Identity;
using Flowdesks.Application.Responses.Identity;
using Flowdesks.Shared.Wrapper;

namespace Flowdesks.Application.Interfaces.Identity
{
    public interface IPermissionService:IService
    {
        Task<Result<PaginatedResult<PermissionsResponse>>> GetAllAsync(PermissionPagingRequest request);
    }
}
