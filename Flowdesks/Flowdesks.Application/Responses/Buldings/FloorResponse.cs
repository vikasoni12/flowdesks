namespace Flowdesks.Application.Responses.Buldings
{
    public class LocationResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Floor { get; set; } = null;
        public Guid BuildingId { get; set; }
        public string? BuildingName {  get; set; } = null;

        public Guid? SiteId { get; set; }
        public string? SiteName { get; set; } = null;
    }
}
