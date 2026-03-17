using Flowdesks.Application.Requests.Asset;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.Asset;

namespace Flowdesks.Application.Specifications.Asset;

public class AssetConditionFilterSpecification : Specification<AssetCondition>
{
    public AssetConditionFilterSpecification(AssetConditionPagingRequest request)
    {
        if (!string.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.Name.Contains(request.StringSearch));
        }
    }
}