using Flowdesks.Application.Responses.Asset;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Asset;

public class GetAssetByIdRequest : IRequest<Result<AssetResponse>>
{
    public Guid AssetId { get; set; }
}
