using System.ComponentModel;

namespace Flowdesks.Application.Responses.Stocks
{
    public class ExportStockResponse
    {
        [Description("Part Code")]
        public string PartCode { get; set; }
        [Description("Part Name")]
        public string PartName { get; set; }
        [Description("Manufacturer")]
        public string Manufacturer { get; set; }

        [Description("Category")]
        public string CategoryName { get; set; }

        [Description("Building")]
        public string BuildingName { get; set; }

        [Description("Location")]
        public string LocationName { get; set; }
        [Description("Supplier")]
        public string SupplierName { get; set; }
        [Description("Bin")]
        public string Bin { get; set; }
        [Description("Description")]
        public string Description { get; set; }
        [Description("Quantity")]
        public decimal? Quantity { get; set; }
        [Description("Unit Cost")]
        public decimal? UnitCost { get; set; }
        [Description("Minimum Quantity")]
        public decimal? MinQuantity { get; set; }
    }
}
