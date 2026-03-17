using Flowdesks.Application.Requests.WorkOrder.ComplianceType;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.SystemPreferences.WorkOrder;

namespace Flowdesks.Application.Specifications.WorkOrder
{
    public  class ComplianceTypeFilterSpecification : Specification<ComplianceType>
    {
        public ComplianceTypeFilterSpecification(ComplianceTypePagingRequest request)
        {
            if (!string.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.Name.Contains(request.StringSearch));
            }
        }
    }
}
