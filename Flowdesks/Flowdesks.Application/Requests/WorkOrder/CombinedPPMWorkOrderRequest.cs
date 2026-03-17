namespace Flowdesks.Application.Requests.WorkOrder;

public class CombinedPPMWorkOrderRequest : PagedRequest
{
    public Guid? AssetId { get; set; }
}