using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.PurchaseOrders.Invoices;

public class CreateXeroInvoiceRequest : IRequest<Result<int>>
{
    public Guid InvoiceId { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string Description { get; set; }
    public decimal TotalCost { get; set; }
    public string FilePath { get; set; }
}