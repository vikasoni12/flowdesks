namespace Flowdesks.Application.Responses.Technicians.Building
{
    public class BuildingForTechnicianResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string? Name { get; set; }            
        public Guid? SiteId { get; set; }
        public string? SiteName { get; set; }
    }
}
