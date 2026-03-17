using Flowdesks.Application.Responses.WorkRequests;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.WorkRequests;

public class SelfServiceUpdatePagingRequest : PagedRequest, IRequest<Result<PaginatedResult<SelfServiceUpdateResponse>>>
{
    public List<Guid>? BuildingIds { get; set; }
}