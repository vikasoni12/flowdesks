using Flowdesks.Shared.Enums;

namespace Flowdesks.Application.Responses.PurchaseOrders
{
    public class PurchaseOrderResponse
    {
        public Guid Id {  get; set; }
        public string PONumber { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Category {  get; set; }
        public Guid? SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public Guid? PartCodeId { get; set; }
        public string PartName { get; set; }
        public int? UnitQuantity { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? TotalCost { get; set; }
        public string? DeliveryPoint { get; set; }
        public string? Status { get; set; } // Status Enum
        public string? Description { get; set; }
        public DateTime? RaiseDate { get; set; }
        public string? RaiseBy { get; set; }
        public string? CreatedBy { get; set; }
        public string? ApproverId { get; set; }
        public string? Approver { get; set; }
    }
}
