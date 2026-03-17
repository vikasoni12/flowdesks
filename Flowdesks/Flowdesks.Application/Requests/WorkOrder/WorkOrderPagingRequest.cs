namespace Flowdesks.Application.Requests.WorkOrder
{
    public class WorkOrderPagingRequest : PagedRequest
    {
        public string? WorkOrderId { get; set; }
        public Guid? UserId { get; set; }
        public List<Guid>? BuildingIds { get; set; }
        public List<Guid>? AssetIds { get; set; }
        public List<Guid>? PriorityIds { get; set; }
        public List<Guid>? CategoryIds { get; set; }
        public List<Guid>? SupplierIds { get; set; }
        public bool? ShowPastDue { get; set; }
        public List<string>? Status { get; set; }
        public bool? IsFromHistory { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CalendarDate { get; set; }
        public bool? IsAccepted { get; set; }
        public List<Guid>? AssignedUserIds { get; set; }
    }
}