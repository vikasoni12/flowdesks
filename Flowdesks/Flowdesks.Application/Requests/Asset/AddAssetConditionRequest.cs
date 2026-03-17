using Flowdesks.Domain.Entities.Asset;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Asset;

public class AddAssetConditionRequest : CreateEditRequest<AssetCondition>, IRequest<Result<int>>
{
    public string Name { get; set; }
    public int Order { get; set; }
}
