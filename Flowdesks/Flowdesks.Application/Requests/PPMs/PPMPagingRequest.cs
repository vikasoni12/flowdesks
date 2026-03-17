namespace Flowdesks.Application.Requests.PPMs
{
    public class PPMPagingRequest : PagedRequest
    {
        public string? PPMId { get; set; }
        public List<Guid>? AssetIds { get; set; }
        public List<Guid>? Priorities { get; set; }
        public List<Guid>? BuildingIds { get; set; }
        public List<Guid>? Compliances { get; set; }
        public string Status { get; set; }
        public List<string>? Frequencies { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public bool IsFromHistory { get; set; } = false;
    }
}
