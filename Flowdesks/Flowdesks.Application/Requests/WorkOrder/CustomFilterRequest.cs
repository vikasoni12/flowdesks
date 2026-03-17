using Flowdesks.Shared.Constants;

namespace Flowdesks.Application.Requests.WorkOrder
{
    public class CustomFilterRequest
    {
        public List<Guid>? BuildingIds { get; set; }
        public List<Guid>? SiteIds { get; set; }
        public DateTime? From { get; set; }
        public virtual DateTime? To { get; set; }
        public string? UserId { get; set; }
        public string Status { get; set; } = OrderStatus.Completed; // Default status
        public DateTime? DueDate { get; set; }
        public bool IsFromHistory { get; set; } = false;
        public bool IsForApprovedForPayment { get; set; } =false;
    }
}