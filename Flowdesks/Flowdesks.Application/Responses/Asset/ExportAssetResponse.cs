using System.ComponentModel;

namespace Flowdesks.Application.Responses.Asset;

public class ExportAssetResponse
{
    [Description("Code")]
    public string Code { get; set; }

    [Description("Name")]
    public string? Name { get; set; }

    [Description("Description")]
    public string? Description { get; set; }

    [Description("Manufacturer")]
    public string? Manufacturer { get; set; }

    [Description("Model")]
    public string? Model { get; set; }

    [Description("Serial Number")]
    public string? SerialNumber { get; set; }

    [Description("Bar Code")]
    public string? BarCode { get; set; }

    [Description("Type")]
    public string? Type { get; set; }

    [Description("Site")]
    public string? Site { get; set; }

    [Description("Building")]
    public string? Building { get; set; }

    [Description("Location")]
    public string? Location { get; set; }

    [Description("Supplier")]
    public string? Supplier { get; set; }

    [Description("Purchase Cost")]
    public decimal? PurchaseCost { get; set; }

    [Description("Current Value")]
    public decimal? CurrentValue { get; set; }

    [Description("Disposal Value")]
    public decimal? DisposalValue { get; set; }

    [Description("Replacement Cost")]
    public decimal? ReplacementCost { get; set; }

    [Description("Life Span")]
    public string? LifeSpan { get; set; }

    [Description("Operational")]
    public string? Operational { get; set; }

    [Description("Health or Safety")]
    public string? HealthOrSafety { get; set; }

    [Description("Condition")]
    public string? Condition { get; set; }

    [Description("Purchase Date")]
    public DateTime? PurchaseDate { get; set; }

    [Description("Warranty Expires Date")]
    public DateTime? WarrantyExpiresDate { get; set; }

    [Description("Last Assessed")]
    public DateTime? LastAssessed { get; set; }
}

