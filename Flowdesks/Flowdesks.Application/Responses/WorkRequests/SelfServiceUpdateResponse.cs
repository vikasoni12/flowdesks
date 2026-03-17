namespace Flowdesks.Application.Responses.WorkRequests;

public class SelfServiceUpdateResponse
{
    public Guid Id { get; set; }
    public Guid SiteId { get; set; }
    public string Site { get; set; }
    public Guid BuildingId { get; set; }
    public string Building { get; set; }
    public string Comment { get; set; }
    public DateTime ModifiedOn { get; set; }
}