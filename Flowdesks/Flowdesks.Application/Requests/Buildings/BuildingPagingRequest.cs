namespace Flowdesks.Application.Requests.Buildings;

public class BuildingPagingRequest : PagedRequest
{
    public List<Guid>? BuildingIds { get; set; }
    public List<Guid>? SiteIds { get; set; }
    public List<Guid>? UserIds { get; set; }
    public List<Guid>? BuildingTypeIds { get; set; }
    public List<string>? OccupancyTypes { get; set; }
    public string? Code { get; set; }
    public Guid? TechnicianId { get; set; }
}