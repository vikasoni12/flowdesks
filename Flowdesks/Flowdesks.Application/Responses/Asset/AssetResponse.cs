namespace Flowdesks.Application.Responses.Asset;

public class AssetResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public string? SerialNumber { get; set; }
    public string? BarCode { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public Guid? TypeId { get; set; }
    public string? Type { get; set; }
    public Guid? SiteId { get; set; }
    public string? SiteName {  get; set; }
    public Guid? BuildingId { get; set; }
    public string? BuildingName {  get; set; }
    public Guid? LocationId { get; set; }
    public string? Location { get; set; }
    public Guid? SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public string Status { get; set; }
    public int TaskCount { get; set; }
    public decimal TotalCost { get; set; }

    public AssetHealthAndFinanceDetailResponse HealthAndFinanceDetail { get; set; } = new();
    public AssetHierarchyResponse AssetHierarchy { get; set; } = new();
}

public class AssetWrapperResponse
{
    public Domain.Entities.Assets.Asset Asset { get; set; }
    public ICollection<Domain.Entities.WorkOrder.WorkOrder> WorkOrders { get; set; }
}
