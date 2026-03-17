namespace Flowdesks.Application.Requests.WorkRequests;

public class WorkRequestPagingRequest : PagedRequest
{
    public string? Id { get; set; }
    public List<Guid>? BuildingIds { get; set; }
    public List<string>? Status { get; set; }
}