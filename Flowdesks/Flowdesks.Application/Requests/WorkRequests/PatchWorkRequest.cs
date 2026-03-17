namespace Flowdesks.Application.Requests.WorkRequests;

public class PatchWorkRequest
{
    public Guid? Id { get; set; }
    public string Reporter { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Problem { get; set; }
    public string Description { get; set; }
    public string DocumentUrl { get; set; }
    public string Status { get; set; }
    public string Comment { get; set; }
    public Guid? BuildingId { get; set; }
    public Guid? LocationId { get; set; }
    public Guid? WorkOrderId { get; set; }
}