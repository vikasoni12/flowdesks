using Flowdesks.Application.Responses.WorkOrder.RequestSource;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.WorkOrder.RequestSource
{
    public class WORequestSourcePagingRequest : FilterPagedRequest, IRequest<Result<PaginatedResult<WORequestSourceResponse>>>
    {    
    }
}
