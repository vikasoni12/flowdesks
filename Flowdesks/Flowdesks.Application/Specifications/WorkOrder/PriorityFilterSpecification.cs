using Flowdesks.Application.Requests.WorkOrder.Priority;
using Flowdesks.Application.Specifications.Base;

namespace Flowdesks.Application.Specifications.WorkOrder
{
    public class PriorityFilterSpecification : Specification<Domain.Entities.SystemPreferences.WorkOrder.Priority>
    {
        public PriorityFilterSpecification(PriorityPagingRequest request) { }
    }
}
