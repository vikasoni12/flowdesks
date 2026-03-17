namespace Flowdesks.Application.Requests.Asset;

public class AssetPagingRequest : FilterPagedRequest
{
    public string? Code {  get; set; }
    public List<Guid>? LocationIds { get; set; }
    public List<Guid>? TypeIds { get; set; }
    public List<Guid>? SupplierIds { get; set; }
    public List<Guid>? ConditionIds { get; set; }
    public string Status { get; set; }
    public bool IsHealthRequired { get; set; } = false;
}