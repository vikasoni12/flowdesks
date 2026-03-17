namespace Flowdesks.Application.Requests.Supplier
{
    public class SupplierPagingRequest : PagedRequest
    {
        public string? Code { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Status { get; set; }
    }
}
