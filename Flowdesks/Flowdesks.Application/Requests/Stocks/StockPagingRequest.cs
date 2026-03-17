namespace Flowdesks.Application.Requests.Stocks
{
    public class StockPagingRequest : PagedRequest
    {
        public List<Guid>? BuildingIds { get; set; }
        public List<Guid>? CategoryIds { get; set; }
        public List<Guid>? SupplierIds { get; set; }
    }
}
