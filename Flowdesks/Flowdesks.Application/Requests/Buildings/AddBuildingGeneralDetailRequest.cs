using Flowdesks.Domain.Entities.Buildings;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Buildings;

public class AddBuildingGeneralDetailRequest : CreateEditRequest<BuildingGeneralDetail>, IRequest<Result<int>>
{
    public Guid? BuildingId { get; set; }
    public int? NumberOfStaff { get; set; }
    public int? NumberOfFloors { get; set; }
    public int? NumberOfLifts { get; set; }
    public int? NumberOfEscalators { get; set; }
    public int? NumberOfEntrances { get; set; }
    public int? NumberOfParkingBays { get; set; }
    public int? NumberOfExits { get; set; }
    public string? GrossExternal { get; set; }
    public string? GrossInternal { get; set; }
    public string? NetInternal { get; set; }
    public string? TotalLettable { get; set; }
    public string? BuildingInsurer { get; set; }
    public string? InsuranceValue { get; set; }
    public string? FireCertificate { get; set; }
    public DateTime? ExpiryDate { get; set; }
}