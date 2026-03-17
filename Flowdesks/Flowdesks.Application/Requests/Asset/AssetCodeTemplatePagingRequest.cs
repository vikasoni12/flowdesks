using Flowdesks.Application.Responses.Asset;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Asset
{
    public class AssetCodeTemplatePagingRequest : PagedRequest,IRequest<Result<PaginatedResult<AssetCodeTemplateResponse>>>
    {
    }
}
