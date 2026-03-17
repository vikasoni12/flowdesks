using Flowdesks.Application.Requests.Support;
using Flowdesks.Application.Specifications.Base;

namespace Flowdesks.Application.Specifications.Support;

public class SupportResponseFilterSpecification : Specification<Domain.Entities.Support.SupportResponse>
{
    public SupportResponseFilterSpecification(SupportResponsePagingRequest request) 
    {
        if (!string.IsNullOrEmpty(request.SupportId))
        {
            And(p => request.SupportId.Equals(p.Support.Id.ToString()));
        }
    }
}