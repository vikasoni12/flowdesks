using System.ComponentModel;

namespace Flowdesks.Application.Responses.Quotes;

public class ExportQuoteResponse
{
    [Description("Quote Number")]
    public string? QuoteNumber { get; set; }
    [Description("Job Type")]
    public string? JobType { get; set; }
    [Description("Category")]
    public string? CategoryName { get; set; }
    [Description("Site")]
    public string? SiteName { get; set; }
    [Description("Building")]
    public string? BuildingName { get; set; }
    [Description("Created Date")]
    public DateTime? CreatedOn { get; set; }
}
