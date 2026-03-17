namespace Flowdesks.Application.Responses.Buldings
{
    public class FloorResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid BuildingId { get; set; }
    }
}
