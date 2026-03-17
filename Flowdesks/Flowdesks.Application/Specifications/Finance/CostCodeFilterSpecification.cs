using Flowdesks.Application.Requests;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.SystemPreferences.Finance;

namespace Flowdesks.Application.Specifications.Finance
{
    public class CostCodeFilterSpecification : Specification<CostCode>
    {
        public CostCodeFilterSpecification(FilterPagedRequest request)
        {
            if (!string.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.Name.Contains(request.StringSearch));
            }
        }
    }
}
