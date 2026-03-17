using Flowdesks.Application.Responses.Asset;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Asset;

public class AssetTypePagingRequest : PagedRequest, IRequest<Result<PaginatedResult<AssetTypeResponse>>>
{
    public List<Guid>? BuildingIds { get; set; }
    public List<Guid>? SiteIds { get; set; }
    public DateTime? From { get; set; }
    public virtual DateTime? To { get; set; }
}