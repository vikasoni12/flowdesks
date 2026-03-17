namespace Flowdesks.Application.Requests.Technicians
{
    public class TechnicianBuildingRequest
    {      
        public Guid TechnicianId { get; set; }
        public Guid BuildingId { get; set; }
    }
}
