namespace Flowdesks.Application.Requests.Support;

public class SupportPagingRequest : PagedRequest
{
    public List<Guid>? PriorityIds { get; set; }
    public List<string>? Status { get; set; }
}