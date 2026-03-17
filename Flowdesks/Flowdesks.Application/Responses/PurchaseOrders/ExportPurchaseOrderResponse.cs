using System.ComponentModel;

namespace Flowdesks.Application.Responses.PurchaseOrders
{
    public class ExportPurchaseOrderResponse
    {
        [Description("PO Number")]
        public string PONumber { get; set; }

        [Description("Category")]
        public string? Category { get; set; }

        [Description("Supplier")]
        public string? SupplierName { get; set; }
        [Description("Part Code")]
        public string? PartName { get; set; }
        [Description("Unit Quantity")]
        public int? UnitQuantity { get; set; }
        [Description("Unit Cost")]
        public decimal? UnitCost { get; set; }
        [Description("Total Cost")]
        public decimal? TotalCost { get; set; }
        [Description("Delivery Point")]
        public string? DeliveryPoint { get; set; }
        [Description("Description")]
        public string? Description { get; set; }
        [Description("Raise Date")]
        public DateTime? RaiseDate { get; set; }  

    }
}
