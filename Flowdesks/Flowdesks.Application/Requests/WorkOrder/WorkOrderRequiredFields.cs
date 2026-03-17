using Flowdesks.Application.Attributes;
using System.ComponentModel;

namespace Flowdesks.Application.Requests.WorkOrder
{
    public class WorkOrderRequiredFields
    {
        [Description("Reporter")]
        [IsRequiredField(true)]
        public string Reporter { get; set; }

        [Description("Email")]
        [IsRequiredField(true)]
        public string Email { get; set; }

        [Description("Category")]
        public Guid WorkOrderCategoryId { get; set; }

        [Description("Phone")]
        public string Phone { get; set; }

        [Description("Problem")]
        [IsRequiredField(true)]
        public string Problem { get; set; }

        [Description("Cost Centre")]
        public Guid? CostCentreId { get; set; }

        [Description("Request Source")]
        public Guid? RequestSourceId { get; set; }

        [Description("CostCode")]
        public Guid? CostCodeId { get; set; }

        [Description("Respond")]
        public string Respond { get; set; }

        [Description("Attend")]
        public string Attend { get; set; }

        [Description("Finished")]
        public string Finished { get; set; }

        [Description("Raised Date")]
        public DateTime? RaisedDate { get; set; }

        [Description("Due Date")]
        [IsRequiredField(true)]
        public DateTime? DueDate { get; set; }

        [Description("Scheduled Date")]
        public DateTime? ScheduledDate { get; set; }

        [Description("Asset")]
        //[IsRequiredField(true)]
        public Guid? AssetId { get; set; }

        [Description("Priority")]
        [IsRequiredField(true)]
        public Guid? PriorityId { get; set; }

        [Description("Building")]
        [IsRequiredField(true)]
        public Guid? BuildingId { get; set; }

        [Description("Location")]
        //[IsRequiredField(true)]
        public Guid? LocationId { get; set; }

        [Description("Assign To")]
        public Guid? AssignTo { get; set; }
    }
}
