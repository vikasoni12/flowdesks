using Flowdesks.Application.Requests.Quotes;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Shared.Enums;

namespace Flowdesks.Application.Specifications.TechnicianQuote;

public class TechnicianQuoteFilterSpecification : Specification<Domain.Entities.Quotes.TechnicianQuote>
{
    public TechnicianQuoteFilterSpecification(TechnicianQuotePagingRequest request)
    {
        if (request.QuoteId != null)
        {
            And(p => p.QuoteId != null && request.QuoteId.Equals((Guid)p.QuoteId));
        }

        And(p => !p.Status.Equals(QuoteStatus.Requested.ToString()));

        if (!string.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.Response.Contains(request.StringSearch));
        }
    }
}
