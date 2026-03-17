namespace Flowdesks.Application.Requests.WorkOrder;

public class DeleteWorkOrderRequest
{
    public List<Guid> Ids { get; set; }
    public bool IsHistorical { get; set; }
}