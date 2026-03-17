namespace Flowdesks.Application.Requests.Technicians
{
    public class DeleteTechnicianBuildingRequest
    {
        public List<Guid>? Ids { get; set; }
        public Guid? TechnicianId { get; set; }
    }
}
