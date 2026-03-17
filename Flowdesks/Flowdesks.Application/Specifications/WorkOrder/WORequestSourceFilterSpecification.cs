using Flowdesks.Application.Requests.WorkOrder.RequestSource;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.SystemPreferences.WorkOrder;

namespace Flowdesks.Application.Specifications.WorkOrder
{
    public  class WORequestSourceFilterSpecification : Specification<WORequestSource>
    {
        public WORequestSourceFilterSpecification(WORequestSourcePagingRequest request)
        {
            if (!string.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.Name.Contains(request.StringSearch));
            }
        }
    }
}
