using Flowdesks.Application.Responses.PPMs;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.PPMs
{
    public class FrequencyColorPagingRequest : PagedRequest, IRequest<Result<PaginatedResult<FrequencyColorResponse>>>
    {
    }
}
