namespace Flowdesks.Application.Requests.Technicians
{
    public class DeleteTechnicianSkillRequest
    {
        public List<Guid>? Ids { get; set; }
        public Guid? TechnicianId { get; set; }
    }
}
