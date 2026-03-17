namespace Flowdesks.Application.Requests.WorkOrder.TimeRecord
{
    public class TimeRecordPagingRequest : PagedRequest
    {
        public Guid? WorkOrderId { get; set; }
    }
}