using Flowdesks.Application.Requests.Asset;
using Flowdesks.Application.Specifications.Base;

namespace Flowdesks.Application.Specifications.Asset;

public class BuildingAssetFilterSpecificationcs : Specification<Domain.Entities.Assets.Asset>
{
    public BuildingAssetFilterSpecificationcs(GetAssetsByBuildingIdRequest request)
    {
        And(x => x.BuildingId == request.BuildingId && !x.IsDeleted);

        if (!string.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.Name.Contains(request.StringSearch) ||
            p.Code.Contains(request.StringSearch) ||
            p.Site.Name.Contains(request.StringSearch) ||
            p.Building.Name.Contains(request.StringSearch) ||
            p.Location.Name.Contains(request.StringSearch) ||
            p.Description.Contains(request.StringSearch));
        }
    }
}