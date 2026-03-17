namespace Flowdesks.Application.Requests.Asset;

public class GetAssetsByBuildingIdRequest : PagedRequest
{
    public Guid BuildingId { get; set; }
}