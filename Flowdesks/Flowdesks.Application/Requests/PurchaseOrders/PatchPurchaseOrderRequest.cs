namespace Flowdesks.Application.Requests.PurchaseOrders;

public class PatchPurchaseOrderRequest
{
    public Guid Id { get; set; }
    public string PONumber { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? SupplierId { get; set; }
    public Guid? PartCodeId { get; set; }
    public int? UnitQuantity { get; set; }
    public decimal? UnitCost { get; set; }
    public decimal? TotalCost { get; set; }
    public string? DeliveryPoint { get; set; }
    public string? Description { get; set; }
    public DateTime? RaiseDate { get; set; }
    public string? RaiseBy { get; set; }
    public Guid? ApproverId { get; set; }
    public int? StockQuantity { get; set; }
    public string? Status { get; set; }
}