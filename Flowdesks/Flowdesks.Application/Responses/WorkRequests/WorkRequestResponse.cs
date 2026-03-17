namespace Flowdesks.Application.Responses.WorkRequests;

public class WorkRequestResponse
{
    public Guid Id { get; set; }
    public string Reporter { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Problem { get; set; }
    public string Description { get; set; }
    public string DocumentUrl { get; set; }
    public string DocumentName { get; set; }
    public string Status { get; set; }
    public string Comment { get; set; }
    public Guid? BuildingId { get; set; }
    public string Building { get; set; }
    public Guid? LocationId { get; set; }
    public Guid? FloorId { get => LocationId; set => LocationId = value; }
    public string Location { get; set; }
    public string Floor { get; set; }
    public Guid? WorkOrderId { get; set; }
    public WorkOrder.WorkOrderResponse WorkOrderResponse { get; set; }
}