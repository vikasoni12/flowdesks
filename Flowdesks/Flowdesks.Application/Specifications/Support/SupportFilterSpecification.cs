using Flowdesks.Application.Requests.Support;
using Flowdesks.Application.Specifications.Base;

namespace Flowdesks.Application.Specifications.Support;

public class SupportFilterSpecification : Specification<Domain.Entities.Support.Support>
{
    public SupportFilterSpecification(SupportPagingRequest request)
    {
        if (request.PriorityIds != null && request.PriorityIds.Count > 0)
        {
            And(p => p.PriorityId != null && request.PriorityIds.Contains((Guid)p.PriorityId));
        }

        if (request.Status != null && request.Status.Count > 0)
        {
            And(p => p.Status != null && request.Status.Contains(p.Status));
        }

        if (!string.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.Title.Contains(request.StringSearch) ||
            p.Description.Contains(request.StringSearch));
        }
    }
}