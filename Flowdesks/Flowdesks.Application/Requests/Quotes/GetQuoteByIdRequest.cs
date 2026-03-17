using Flowdesks.Application.Responses.Quotes;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Quotes
{
    public class GetQuoteByIdRequest : IRequest<Result<QuoteResponse>>
    {
        public string QuoteId { get; set; }
    }
}
