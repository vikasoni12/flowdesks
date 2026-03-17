using Flowdesks.Application.Attributes;
using System.ComponentModel;

namespace Flowdesks.Application.Requests.Asset
{
    public class AssetRequiredFields
    {
        [IsRequiredField(true)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }

        [Description("Serial Number")]
        public string? SerialNumber { get; set; }

        [Description("Bar Code")]
        public string? BarCode { get; set; }

        [IsRequiredField(true)]
        [Description("Type")]
        public string? TypeId { get; set; }

        [IsRequiredField(true)]
        [Description("Site")]
        public string? SiteId { get; set; }

        [IsRequiredField(true)]
        [Description("Building")]
        public string? BuildingId { get; set; }

        [IsRequiredField(true)]
        [Description("Location")]
        public string? LocationId { get; set; }

        [Description("Supplier")]
        public string? SupplierId { get; set; }
      
        [Description("Parent asset")]
        public Guid? ParentAssetId { get; set; }

        [Description("Purchase date")]
        public DateTime? PurchaseDate { get; set; }

        [Description("Purchase cost")]
        public decimal? PurchaseCost { get; set; }

        [Description("Lifespan")]
        public decimal? LifeSpan { get; set; }

        [Description("Warranty Expires Date")]
        public DateTime? WarrantyExpiresDate { get; set; }

        [Description("Profile picture")]
        public string? ProfilePicture { get; set; }
    }
}
