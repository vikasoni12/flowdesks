using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Quotes
{
    public class CreateQuoteRequest : CreateEditRequest<Domain.Entities.Quotes.Quote>, IRequest<Result<int>>
    {
        public string QuoteNumber { get; set; }
        public string JobType { get; set; }
        public Guid? WorkOrderCategoryId { get; set; }
        public Guid? BuildingId { get; set; }
        public Guid? SiteId { get; set; }
        public decimal? Cost { get; set; }
        public QuoteStatus Status { get; set; } = QuoteStatus.Requested;
        public DateTime? JobDate { get; set; }
        public string JobDetails { get; set; }
        public List<SendToQuoteRequest> SendToIds { get; set; }
        public UploadByteArray ProfilePicture { get; set; } = new();
    }
}