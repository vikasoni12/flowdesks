namespace Flowdesks.Application.Requests.PPMs;

public class PatchPPMRequest
{
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
    public string Problem { get; set; }
    public decimal? StockCost { get; set; }
    public decimal? LabourCost { get; set; }
    public decimal? TotalCost { get; set; }
    public bool IsPermited { get; set; }
    public int? EstimateTime { get; set; }
    public DateTime? NextServiceDate { get; set; }
    public string Frequency { get; set; }
    public int? FrequencyOccurrence { get; set; }
    public Guid? ComplianceTypeId { get; set; }
    public Guid? CostCentreId { get; set; }
    public Guid? CostCodeId { get; set; }
    public decimal? Period { get; set; }

    public Guid? PriorityId { get; set; }
    public Guid? InstructionId { get; set; }
    public Guid? AssetId { get; set; }
    public Guid? BuildingId { get; set; }
    public Guid? LocationId { get; set; }
    public Guid? SupplierId { get; set; }
    public List<PPMSkills>? SkillIds { get; set; }
    public Guid? TechnicianId { get; set; }
    public bool IsStatusModified { get; set; }
    public string Status { get; set; }
    public SuspendedPPMPatchRequest? SuspendedPPM { get; set; }
    public PPMStatusTrackerPatchRequest? PPMStatusTrackerRequest { get; set; }
    public Guid? AssignTo { get; set; }
    public Guid? ProcedureId { get; set; }
}

public class PPMSkills
{
    public Guid? SkillId { get; set; }
}

public class SuspendedPPMPatchRequest
{
    public DateTime? SuspendedFrom { get; set; }
    public DateTime? SuspendedTill { get; set; }
    public bool IsVisible { get; set; } = true;
}

public class PPMStatusTrackerPatchRequest
{
    public DateTime? RaisedDate { get; set; }
    public string Status { get; set; }
    public bool IsHistorical { get; set; }
}