namespace Flowdesks.Application.Responses.Quotes
{
    public class QuoteResponse
    {
        public Guid Id { get; set; }
        public string QuoteNumber { get; set; }
        public string JobType { get; set; }
        public Guid? CategoryId { get; set; }
        public string  CategoryName { get; set; }
        public Guid? BuildingId { get; set; }
        public string BuildingName { get; set; }
        public Guid? SiteId { get; set; }
        public string SiteName { get; set; }
        public decimal? Cost { get; set; }
        public string Status { get; set; }
        public DateTime? JobDate { get; set; }
        public string JobDetails { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ProfilePictureUrl { get; set; }
        public  List<TechnicianQuoteResponse>? SendToQuotes { get; set; }
    }
}
