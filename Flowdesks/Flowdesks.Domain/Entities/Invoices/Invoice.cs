using Flowdesks.Domain.Common;

namespace Flowdesks.Domain.Entities.Invoices;

public class Invoice : FullAuditableEntity<Guid>
{
    public long InvoiceNumber {  get; set; }
    public DateTime InvoiceDate { get; set; }
    public decimal PartCost { get; set; }
    public decimal LabourCost { get; set; }
    public decimal TotalCost { get; set; }
    public decimal GST {  get; set; }
    public string? UploadInvoiceUrl {  get; set; }
    public string? FileName { get; set; }
    public Guid EntityId { get; set; }
    public string EntityType { get; set; } //EntityType Enum
    public string XeroInvoiceId { get; set; }
}