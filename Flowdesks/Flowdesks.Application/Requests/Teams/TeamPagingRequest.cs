namespace Flowdesks.Application.Requests.Teams;

public class TeamPagingRequest :PagedRequest
{
    public string? IdNumber { get; set; }
    public Guid? BuildingId { get; set; }
    public List<Guid>? SiteIds { get; set; }
    public List<Guid>? UserIds { get; set; }
}
