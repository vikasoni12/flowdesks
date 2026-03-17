using System.ComponentModel;

namespace Flowdesks.Application.Responses.WorkOrder
{
    public class ExportWorkOrderResponse
    {

        [Description("WorkOrderId")]
        public string WorkOrderId { get; set; }

        [Description("Reporter")]
        public string Reporter { get; set; }

        [Description("Email")]
        public string Email { get; set; }

        [Description("Category")]
        public string Category { get; set; }

        [Description("Phone")]
        public string Phone { get; set; }

        [Description("Problem")]
        public string Problem { get; set; }

        [Description("Asset Code")]
        public string AssetCode { get; set; }

        [Description("Cost Center")]
        public string CostCentre { get; set; }

        [Description("Source")]
        public string Source { get; set; }

        [Description("Cost Code")]
        public string CostCode { get; set; }
        [Description("Respond")]
        public string Respond { get; set; }

        [Description("Attend")]
        public string Attend { get; set; }

        [Description("Finished")]
        public string Finished { get; set; }

        [Description("Raised Date")]
        public DateTime? RaisedDate { get; set; }

        [Description("Due Date")]
        public DateTime? DueDate { get; set; }

        [Description("Scheduled Date")]
        public DateTime? ScheduledDate { get; set; }

        [Description("Priority")]
        public string Priority { get; set; }

        [Description("Building")]
        public string BuildingName { get; set; }

        [Description("Location")]
        public string BuildingLocation { get; set; }

        [Description("Supplier")]
        public string SupplierName { get; set; }

        [Description("Technician")]
        public string TechnicianName { get; set; }
    }
}
