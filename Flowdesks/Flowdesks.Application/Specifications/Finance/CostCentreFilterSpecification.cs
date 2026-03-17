using Flowdesks.Application.Requests;
using Flowdesks.Application.Requests.Finance;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.SystemPreferences.Finance;

namespace Flowdesks.Application.Specifications.Finance
{
    public class CostCentreFilterSpecification : Specification<CostCentre>
    {
        public CostCentreFilterSpecification(CostCentrePagingRequest request)
        {
            if (!string.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.Name.Contains(request.StringSearch));
            }
        }

        public CostCentreFilterSpecification(FilterPagedRequest request)
        {
            if (!string.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.Name.Contains(request.StringSearch));
            }
        }
    }
}
