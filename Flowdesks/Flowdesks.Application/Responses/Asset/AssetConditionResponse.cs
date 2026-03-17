namespace Flowdesks.Application.Responses.Asset;

public class AssetConditionResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public int AssetCount { get; set; }
}