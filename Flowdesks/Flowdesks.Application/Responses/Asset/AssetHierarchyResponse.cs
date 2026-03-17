namespace Flowdesks.Application.Responses.Asset;

public class AssetHierarchyResponse
{
    public Guid? ParentAssetId { get; set; }
    public AssetResponse? ParentAsset { get; set; }

    public IEnumerable<AssetResponse>? ChildAssets { get; set; }
}
