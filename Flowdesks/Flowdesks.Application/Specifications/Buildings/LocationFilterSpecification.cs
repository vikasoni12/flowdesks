using Flowdesks.Application.Requests.Buildings;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.Buildings;

namespace Flowdesks.Application.Specifications.Buildings;

public class LocationFilterSpecification : Specification<BuildingLocation>
{
    public LocationFilterSpecification(LocationPagingRequest request)
    {
        And(x => !x.IsDeleted);

        if (request.BuildingId != Guid.Empty && request.BuildingId != null)
        {
            And(p => p.BuildingId.Equals(request.BuildingId));
        }

        if (!string.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.Name.Contains(request.StringSearch)
            || p.Description.Contains(request.StringSearch));
        }
    }
}