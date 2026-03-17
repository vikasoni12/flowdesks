using Flowdesks.Application.Requests.WorkRequests;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.WorkRequests;

namespace Flowdesks.Application.Specifications.WorkRequests;

public class SelfServiceUpdateFilterSpecification : Specification<SelfServiceUpdate>
{
    public SelfServiceUpdateFilterSpecification(SelfServiceUpdatePagingRequest request)
    {
        if (request.BuildingIds != null && request.BuildingIds.Count > 0)
        {
            And(p => request.BuildingIds.Contains((Guid)p.BuildingId));
        }
    }
}