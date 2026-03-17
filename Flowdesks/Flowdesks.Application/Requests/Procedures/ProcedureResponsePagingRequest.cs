using Flowdesks.Application.Responses.Procedures;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Procedures;

public class ProcedureResponsePagingRequest : PagedRequest, IRequest<Result<PaginatedResult<ProcedurePagingResponse>>>
{
    public string? Id { get; set; }
    public List<Guid>? AssetTypeIds { get; set; }
    public string? Name { get; set; }
    public string? Category { get; set; }
    public string? EntityId { get; set; }
}