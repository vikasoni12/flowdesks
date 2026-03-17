namespace Flowdesks.Application.Responses.Buldings;

public class BuildingResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? OccupancyType { get; set; } 
    public string? Description { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public Guid? CostCentreId { get; set; }
    public string? CostCentre { get; set; }
    public Guid? SiteId { get; set; }
    public string? SiteName { get; set; }
    public Guid? BuildingTypeId { get; set; }
    public string? BuildingTypeName { get; set; }
    public int? WorkOrdersCount { get; set; }

    public IEnumerable<BuildingDailyScheduleResponse> BuildingDailySchedules { get; set; }
    public BuildingGeneralDetailResponse GeneralDetails { get; set; } = new();
}
