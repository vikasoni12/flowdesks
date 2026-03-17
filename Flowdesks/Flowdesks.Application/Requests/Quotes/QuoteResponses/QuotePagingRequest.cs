namespace Flowdesks.Application.Requests.Quotes;

public class TechnicianQuotePagingRequest : PagedRequest
{
    public Guid? QuoteId { get; set; }
}
