namespace Flowdesks.Application.Requests.Technicians
{
    public class TechnicianPagingRequest : PagedRequest
    {
        public string? IdNumber { get; set; }
        public Guid? BuildingId { get; set; }
        public List<Guid>? SiteIds { get; set; }
        public List<Guid>? SkillIds { get; set; } 
        public List<Guid>? QualificationIds {  get; set; }
        public List<Guid>? SupplierIds { get; set; }
        public bool? IsExternal { get; set; } = false;
        public string? Status { get; set; }
        public List<Guid>? BuildingIds { get; set; }
    }
}
