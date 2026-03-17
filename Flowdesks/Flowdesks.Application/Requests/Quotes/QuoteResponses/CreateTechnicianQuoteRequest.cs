using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Quotes.QuoteResponses
{
    public class CreateTechnicianQuoteRequest : CreateEditRequest<Domain.Entities.Quotes.TechnicianQuote>, IRequest<Result<int>>
    {
        public decimal Cost { get; set; }
        public string Response { get; set; }
        public Guid QuoteId { get; set; }
        public Guid? TechnicianId { get; set; }
        public Guid? SupplierId { get; set; }
        public DateTime? ProposedJobDate { get; set; }
    }
}
