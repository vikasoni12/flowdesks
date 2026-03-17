using Flowdesks.Application.Attributes;
using System.ComponentModel;

namespace Flowdesks.Application.Requests.PPMs;

public class PPMRequiredFields
{
    [Description("Problem")]
    [IsRequiredField(true)]
    public string Problem { get; set; }

    [Description("Stock Cost")]
    [IsRequiredField(true)]
    public decimal StockCost { get; set; }

    [Description("Labour Cost")]
    [IsRequiredField(true)]
    public decimal LabourCost { get; set; }

    [Description("Permit")]
    public bool? IsPermited { get; set; }

    [Description("Estimate Time")]
    [IsRequiredField(true)]
    public int EstimateTime { get; set; }

    [Description("Next Service Date")]
    [IsRequiredField(true)]
    public DateTime NextServiceDate { get; set; }

    [Description("Frequency")]
    [IsRequiredField(true)]
    public string Frequency { get; set; }

    [Description("Frequency Occurrence")]
    public int FrequencyOccurrence { get; set; }

    [Description("Compliance Type")]
    public Guid? ComplianceTypeId { get; set; }

    [Description("Cost Centre")]
    public Guid? CostCentreId { get; set; }

    [Description("Cost Code")]
    public Guid? CostCodeId { get; set; }

    [Description("Priority")]
    [IsRequiredField(true)]
    public Guid PriorityId { get; set; }

    [Description("Asset")]
    [IsRequiredField(true)]
    public Guid AssetId { get; set; }

    [Description("Building")]
    [IsRequiredField(true)]
    public Guid BuildingId { get; set; }

    [Description("Location")]
    [IsRequiredField(true)]
    public Guid LocationId { get; set; }

    [Description("Assign To")]
    public Guid? AssignTo { get; set; }

    [Description("Skills")]
    [IsRequiredField(true)]
    public List<PPMSkillRequest>? SkillIds { get; set; }

    [Description("Procedure")]
    public Guid? ProcedureId { get; set; }
}