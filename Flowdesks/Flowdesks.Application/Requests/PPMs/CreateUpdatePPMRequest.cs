using Flowdesks.Application.Attributes;
using Flowdesks.Domain.Entities.PPM;
using System.ComponentModel;

namespace Flowdesks.Application.Requests.PPMs
{
    public class CreateUpdatePPMRequest : CreateEditRequest<PPM>
    {
        public Guid? ParentId { get; set; }
        public string Problem {  get; set; }
        public decimal StockCost { get; set; }
        public decimal LabourCost { get; set; }
        public decimal TotalCost { get; set; }
        public bool IsPermited { get; set; }
        public int EstimateTime { get; set; }
        public DateTime NextServiceDate { get; set; }
        public string Frequency { get; set; }
        public int FrequencyOccurrence { get; set; }
        public Guid? ComplianceTypeId { get; set; }
        public Guid? CostCentreId { get; set; }
        public Guid? CostCodeId { get; set; }
        public decimal? Period { get; set; }

        public Guid PriorityId { get; set; }
        public Guid? InstructionId { get; set; }
        public Guid AssetId { get; set; }
        public Guid BuildingId { get; set; }
        public Guid LocationId { get; set; }
        public Guid? SupplierId { get; set; }
        public  List<PPMSkillRequest>? SkillIds { get; set; }
        public Guid? TechnicianId { get; set; }
        public Guid? ParentPPMId { get; set; }
        public DateTime? PreviousScheduledDate { get; set; }
        public bool IsStatusModified { get; set; }
        public string? Status { get;set; }
        public SuspendedPPMRequest? SuspendedPPM { get; set; }
        public PPMStatusTrackerRequest? PPMStatusTrackerRequest { get; set; }
        public Guid? AssignTo { get; set; }
        public Guid? ProcedureId { get; set; }
    }

    public class PPMSkillRequest
    {
        public Guid SkillId { get; set; }
    }

    public class SuspendedPPMRequest
    {
        public DateTime SuspendedFrom { get; set; }
        public DateTime? SuspendedTill { get; set; }
        public bool IsVisible { get; set; } = true;
    }
    
    public class PPMStatusTrackerRequest
    {
        public DateTime RaisedDate { get; set; }
        public string Status { get; set; }
        public bool IsHistorical { get; set; }
    }
}
