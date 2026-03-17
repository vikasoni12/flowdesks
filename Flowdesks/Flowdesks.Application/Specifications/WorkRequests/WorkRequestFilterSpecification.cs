using Flowdesks.Application.Requests.WorkOrder;
using Flowdesks.Application.Requests.WorkRequests;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.WorkRequests;

namespace Flowdesks.Application.Specifications.WorkRequests;

public class WorkRequestFilterSpecification : Specification<WorkRequest>
{
    public WorkRequestFilterSpecification(WorkRequestPagingRequest request) {

        if (request.Id != null)
        {
            And(p => p.CreatedBy != null && request.Id.Equals(p.CreatedBy));
        }

        if (request.BuildingIds != null && request.BuildingIds.Count > 0)
        {
            And(p => request.BuildingIds.Contains((Guid)p.BuildingId));
        }

        if (request.Status != null && request.Status.Count > 0)
        {
            And(p => p.Status != null && request.Status.Contains(p.Status));
        }

        if (!string.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.Reporter.Contains(request.StringSearch) ||
            p.Email.Contains(request.StringSearch) ||
            p.Phone.Contains(request.StringSearch) ||
            p.Problem.Contains(request.StringSearch) ||
            p.Building.Name.Contains(request.StringSearch) ||
            p.BuildingLocation.Name.Contains(request.StringSearch)
            );
        }
    }

    public WorkRequestFilterSpecification(CustomFilterRequest request)
    {
        if (request.BuildingIds != null && request.BuildingIds.Count > 0)
        {
            And(p => request.BuildingIds.Contains((Guid)p.BuildingId));
        }

        if (request.SiteIds != null && request.SiteIds.Count > 0)
        {
            And(p => p.Building.SiteId != null && request.SiteIds.Contains((Guid)p.Building.SiteId));
        }

        if (request.From != null)
        {
            And(p => p.CreatedOn.Date >= request.From.Value.Date);
        }

        if (request.To != null)
        {
            And(p => p.CreatedOn.Date <= request.To.Value.Date);
        }
    }
}