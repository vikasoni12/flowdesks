namespace Flowdesks.Application.Requests.Technicians
{
    public class DeleteTechnicianQualificationRequest
    {
        public List<Guid>? Ids { get; set; }
        public Guid? TechnicianId { get; set; }
    }
}
