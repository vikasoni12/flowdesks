using Flowdesks.Application.Responses.Support;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Support;

public class SupportResponsePagingRequest : PagedRequest, IRequest<Result<PaginatedResult<SupportResponsesResponse>>>
{
    public string SupportId { get; set; }
}