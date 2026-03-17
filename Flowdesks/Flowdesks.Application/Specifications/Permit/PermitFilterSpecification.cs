using Flowdesks.Application.Requests.Permit;
using Flowdesks.Application.Specifications.Base;

namespace Flowdesks.Application.Specifications.Permit;

public class PermitFilterSpecification : Specification<Domain.Entities.Permit.Permit>
{
    public PermitFilterSpecification(PermitPagingRequest request)
    {
        if (!string.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.Name.Contains(request.StringSearch));
        }
    }
}