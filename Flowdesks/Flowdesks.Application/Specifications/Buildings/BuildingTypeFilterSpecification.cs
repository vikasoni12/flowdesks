using Flowdesks.Application.Requests.Buildings;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.SystemPreferences.Buildings;

namespace Flowdesks.Application.Specifications.Buildings;

public class BuildingTypeFilterSpecification : Specification<BuildingType>
{
    public BuildingTypeFilterSpecification(BuildingTypePagingRequest request)
    {
        if (!string.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.Name.Contains(request.StringSearch));
        }
    }
}
