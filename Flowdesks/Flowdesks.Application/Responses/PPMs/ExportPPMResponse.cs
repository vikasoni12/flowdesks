using System.ComponentModel;


namespace Flowdesks.Application.Responses.PPMs
{
    public class ExportPPMResponse
    {
        [Description("PPM Id")]
        public string TaskId { get; set; }
        [Description("Stock Cost")]
        public decimal StockCost { get; set; }
        [Description("Labour Cost")]
        public decimal LabourCost { get; set; }
        [Description("Total Cost")]
        public decimal TotalCost { get; set; }
        [Description("Estimated Time")]
        public int EstimateTime { get; set; }
        [Description("Next Service Date")]
        public DateTime NextServiceDate { get; set; }
        [Description("Frequency")]
        public string Frequency { get; set; }
        [Description("FrequencyOccurrence")]
        public string FrequencyOccurrence { get; set; }
        [Description("Compliance Type")]
        public string Compliance { get; set; }
        [Description("Cost Center")]
        public string CostCentre { get; set; }
        [Description("Cost Code")]
        public string CostCode { get; set; }
        [Description("Priority")]
        public string Priority { get; set; }
        [Description("Task Type")]
        public string Asset { get; set; }
        [Description("Building")]
        public string BuildingName { get; set; }
        [Description("Location")]
        public string Location { get; set; }
        [Description("Supplier")]
        public string Supplier { get; set; }
        [Description("Technician")]
        public string Technician { get; set; }
        [Description("Raise Date")]
        public DateTime CreatedOn { get; set; }
    }
}
