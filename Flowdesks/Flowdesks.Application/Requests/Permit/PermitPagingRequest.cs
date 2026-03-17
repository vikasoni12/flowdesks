using Flowdesks.Application.Responses.Permit;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Permit;

public class PermitPagingRequest : FilterPagedRequest, IRequest<Result<PaginatedResult<PermitResponse>>>
{
}
