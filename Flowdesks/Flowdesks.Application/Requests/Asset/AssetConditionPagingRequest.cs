using Flowdesks.Application.Responses.Asset;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Asset;

public class AssetConditionPagingRequest : FilterPagedRequest, IRequest<Result<PaginatedResult<AssetConditionResponse>>>
{
}