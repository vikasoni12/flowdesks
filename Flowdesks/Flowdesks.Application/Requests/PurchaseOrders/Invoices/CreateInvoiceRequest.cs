using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Wrapper;
using MediatR;


namespace Flowdesks.Application.Requests.PurchaseOrders.Invoices
{
    public class CreateInvoiceRequest : IRequest <Result<int>>
    {
        public long InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal PartCost { get; set; }
        public decimal LabourCost { get; set; }
        public decimal? TotalCost
        {
            get
            {
                return PartCost + LabourCost;
            }
        }
        public decimal GST { get; set; }
        public string? UploadInvoiceUrl { get; set; }
        public string? FileName { get; set; }
        public Guid EntityId { get; set; }
        public EntityType EntityType { get; set; }
    }
}