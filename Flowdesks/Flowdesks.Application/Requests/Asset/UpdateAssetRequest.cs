using Flowdesks.Application.Responses.Asset;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Asset;

public class UpdateAssetRequest : IRequest<Result<AssetResponse>>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public string? SerialNumber { get; set; }
    public string? BarCode { get; set; }
    public Guid? TypeId { get; set; }
    public Guid? SiteId { get; set; }
    public Guid? BuildingId { get; set; }
    public Guid? LocationId { get; set; }
    public Guid? SupplierId { get; set; }
    public Guid? ParentAssetId { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string Status { get; set; } 
    public UploadByteArray? ProfilePicture { get; set; } = new();

    public Guid? ConditionId {  get; set; }
    public bool IsMobile { get; set; } = false;
}
