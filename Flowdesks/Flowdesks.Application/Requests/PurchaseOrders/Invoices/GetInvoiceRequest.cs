using Flowdesks.Shared.Enums;


namespace Flowdesks.Application.Requests.PurchaseOrders.Invoices;

public class GetInvoiceRequest
{
    public Guid EntityId { get; set; }
    public EntityType? EntityType { get; set; }
}
