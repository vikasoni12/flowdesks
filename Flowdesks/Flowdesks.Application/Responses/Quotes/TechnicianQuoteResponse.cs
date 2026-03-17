using Flowdesks.Domain.Entities.Chat;

namespace Flowdesks.Application.Responses.Quotes;

public class TechnicianQuoteResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid? TechnicianId { get; set; }
    public Guid? SupplierId { get; set; }
    public Guid? QuoteId { get; set; }
    public decimal? Cost { get; set; }
    public DateTime? RecievedDate { get; set; }
    public DateTime? SentDate { get; set; }
    public string? Status {  get; set; }
    public string? Response {  get; set; }
    public string? AlertMessage {  get; set; }
    public string? ApproverId { get; set; }
    public string? Approver { get; set; }
    public DateTime? ProposedJobDate { get; set; }
}
