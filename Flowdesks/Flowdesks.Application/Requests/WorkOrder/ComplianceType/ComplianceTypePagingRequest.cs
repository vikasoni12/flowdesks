using Flowdesks.Application.Responses.WorkOrder.ComplianceType;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.WorkOrder.ComplianceType;

public class ComplianceTypePagingRequest : PagedRequest, IRequest<Result<PaginatedResult<ComplianceTypeResponse>>>
{
    public List<Guid>? BuildingIds { get; set; }
    public List<Guid>? SiteIds { get; set; }
    public DateTime? From { get; set; }
    public virtual DateTime? To { get; set; }
}