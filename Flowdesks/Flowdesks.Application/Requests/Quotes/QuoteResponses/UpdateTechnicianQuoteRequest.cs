using Flowdesks.Application.Responses.Quotes;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Quotes.QuoteResponses
{
    public class UpdateTechnicianQuoteRequest : CreateEditRequest<Domain.Entities.Quotes.TechnicianQuote>, IRequest<Result<TechnicianQuoteResponse>>
    {
        public Guid Id { get; set; }
        public decimal Cost { get; set; }
        public string Response { get; set; }
        public string Status { get; set; }
        public Guid QuoteId { get; set; }
        public Guid? TechnicianId { get; set; }
        public Guid? SupplierId { get; set; }
        public bool IsNewResponse { get; set; } = false;
        public DateTime? ProposedJobDate { get; set; }
    }
}
