namespace Flowdesks.Application.Requests.Quotes;

public class QuotePagingRequest : PagedRequest
{
    public string? QuoteNumber { get; set; }
    public List<Guid>? BuildingIds { get; set; }
    public List<Guid>? CategoryIds { get; set; }
    public List<string>? Status { get; set; }
}
