namespace Flowdesks.Application.Responses.Stocks
{
    public class StockResponse
    {
        public Guid? Id { get; set; }
        public string PartCode { get; set; }
        public string PartName { get; set; }
        public string Manufacturer { get; set; }
        public Guid? CategoryId { get; set; }
        public string CategoryName {  get; set; }
        public Guid? BuildingId { get; set; }
        public string BuildingName { get; set; }
        public Guid? LocationId { get; set; }
        public string LocationName { get; set; }
        public Guid? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string Bin { get; set; }
        public string Description { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? MinQuantity { get; set; }
        public string? ImageUrl { get; set; }
    }
}
