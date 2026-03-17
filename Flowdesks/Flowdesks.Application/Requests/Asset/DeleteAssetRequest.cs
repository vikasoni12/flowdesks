namespace Flowdesks.Application.Requests.Asset;

public class DeleteAssetRequest
{
    public List<Guid> Ids { get; set; }
    public bool IsHistorical { get; set; }
}
