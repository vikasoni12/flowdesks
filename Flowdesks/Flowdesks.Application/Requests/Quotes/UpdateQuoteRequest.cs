using Flowdesks.Application.Responses.Quotes;
using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Quotes
{
    public class UpdateQuoteRequest : CreateEditRequest<Domain.Entities.Quotes.Quote>, IRequest<Result<QuoteResponse>>
    {
        public Guid? Id { get; set; }
        public string QuoteNumber { get; set; }
        public string JobType { get; set; }
        public Guid? WorkOrderCategoryId { get; set; }
        public Guid? BuildingId { get; set; }
        public Guid? SiteId { get; set; }
        public decimal? Cost { get; set; }
        public string Status { get; set; } = QuoteStatus.Requested.ToString();
        public DateTime? JobDate { get; set; }
        public string JobDetails { get; set; }
        public string ProfilePictureUrl { get; set; }
        public UploadByteArray ImageName { get; set; } = new();
        public List<SendToQuoteRequest> TechnicianQuote { get; set; }
        public List<SendToQuoteRequest> SendToIds { get; set; }
    }
}
