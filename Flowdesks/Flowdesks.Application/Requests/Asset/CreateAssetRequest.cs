using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Asset
{
    public class CreateAssetRequest : CreateEditRequest<Domain.Entities.Assets.Asset>, IRequest<Result<int>>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string? SerialNumber { get; set; }
        public string? BarCode { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public Guid? TypeId { get; set; }
        public Guid? SiteId { get; set; }
        public Guid? BuildingId { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? SupplierId { get; set; }
        public Guid? ParentAssetId { get; set; }
        public decimal? PurchaseCost { get; set; }
        public decimal? LifeSpan { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyExpiresDate { get; set; }
        public AssetStatus Status { get; set; } = AssetStatus.Active;

        public UploadByteArray? ProfilePicture { get; set; } = new();
        public decimal? ReplacementCost { get; set; }
        public Guid? ConditionId { get; set; }
        public string? TempCode {  get; set; }
    }
}
