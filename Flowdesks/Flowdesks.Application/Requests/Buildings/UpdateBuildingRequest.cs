using Flowdesks.Application.Responses.Buldings;
using Flowdesks.Domain.Entities.Buildings;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Buildings;

public class UpdateBuildingRequest : CreateEditRequest<Building>, IRequest<Result<BuildingResponse>>
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? PostCode { get; set; }
    public string? CountryId { get; set; }
    public string? OccupancyType { get; set; } 
    public string? Description { get; set; }
    public Guid? SiteId { get; set; }
    public Guid? BuildingTypeId { get; set; }
    public Guid? CostCentreId { get; set; }
    public virtual IEnumerable<BuildingDailyScheduleRequest>? BuildingDailySchedules { get; set; }

    public string? ProfilePictureUrl { get; set; }
    public UploadByteArray? BuildingImage { get; set; } = new();
}
