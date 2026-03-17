namespace Flowdesks.Application.Requests.Site
{
    public class SitePagingRequest : PagedRequest
    {
        public List<Guid> UserIds { get; set; }
        public List<Guid> CountryId { get; set; }
        public List<Guid>? TeamIds { get; set; }
    }
}