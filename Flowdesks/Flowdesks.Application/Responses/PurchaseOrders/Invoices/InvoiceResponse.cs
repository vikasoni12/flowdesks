using Flowdesks.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowdesks.Application.Responses.PurchaseOrders.Invoices
{
    public class InvoiceResponse
    {
        public Guid Id { get; set; }
        public long InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal PartCost { get; set; }
        public decimal LabourCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal GST { get; set; }
        public string? UploadInvoiceUrl { get; set; }
        public string? FileName { get; set; }
        public Guid? PurchaseOrderId { get; set; }
        public Guid EntityId { get; set; }
        public EntityType? EntityType { get; set; }
    }
}
