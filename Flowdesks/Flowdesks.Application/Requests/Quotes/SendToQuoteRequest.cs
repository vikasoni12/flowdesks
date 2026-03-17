namespace Flowdesks.Application.Requests.Quotes
{
    public class SendToQuoteRequest
    {
        public Guid? TechnicianId { get; set; }
        public Guid? SupplierId { get; set; }
    }
}
