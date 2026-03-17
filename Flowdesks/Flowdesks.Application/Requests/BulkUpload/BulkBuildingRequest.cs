using Flowdesks.Application.Requests.Buildings;
using Flowdesks.Domain.Entities.Buildings;
using Flowdesks.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowdesks.Application.Requests.BulkUpload;
public class BulkBuildingRequest : CreateEditRequest<Building>, IRequest<Result<int>>
{
    public string Code { get; set; }
    public string ICode { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? PostCode { get; set; }
    public Guid? CountryId { get; set; }
    public string? OccupancyType { get; set; }
    public string? Description { get; set; }
    public Guid? SiteId { get; set; }
    public Guid? BuildingTypeId { get; set; }
    public Guid? CostCentreId { get; set; }
    public IEnumerable<BuildingDailyScheduleRequest>? BuildingDailySchedules { get; set; }

    public UploadByteArray? BuildingImage { get; set; } = new();
}
