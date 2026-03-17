using Flowdesks.Application.Requests.WorkOrder.TimeRecord;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.WorkOrder.TimeRecord;

namespace Flowdesks.Application.Specifications.WorkOrder
{
    public class TimeRecordFilterSpecification : Specification<TimeRecord>
    {
        public TimeRecordFilterSpecification(TimeRecordPagingRequest request) {

            if (request.WorkOrderId != null)
            {
                And(p => p.TaskId != null && request.WorkOrderId == (Guid)p.TaskId);
            }
        }
    }
}