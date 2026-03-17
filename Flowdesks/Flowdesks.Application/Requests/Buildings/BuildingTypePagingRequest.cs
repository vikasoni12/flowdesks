using Flowdesks.Application.Responses.Buldings;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Buildings;

public class BuildingTypePagingRequest : PagedRequest, IRequest<Result<PaginatedResult<BuildingTypeResponse>>>
{
}