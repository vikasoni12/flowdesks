using Flowdesks.Domain.Entities.SystemPreferences.Asset;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Asset;

public class AddUpdateAssetTypeRequest : CreateEditRequest<AssetType>, IRequest<Result<int>>
{
    public string Name { get; set; }
}