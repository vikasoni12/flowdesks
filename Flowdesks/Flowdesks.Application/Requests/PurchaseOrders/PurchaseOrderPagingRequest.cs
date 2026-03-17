namespace Flowdesks.Application.Requests.PurchaseOrders;

public class PurchaseOrderPagingRequest : PagedRequest
{
    public List<Guid>? CategoryIds { get; set; }
    public List<Guid>? SupplierIds { get; set; }
    public List<string>? Status { get; set; }
    public string? PONumber { get; set; }
}
