using Flowdesks.Application.Requests.Asset;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.SystemPreferences.Asset;

namespace Flowdesks.Application.Specifications.Asset
{
    public class AssetTypeFilterSpecification : Specification<AssetType>
    {
        public AssetTypeFilterSpecification(AssetTypePagingRequest request)
        {
            if (!string.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.Name.Contains(request.StringSearch));
            }
        }
    }
}
