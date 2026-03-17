using Flowdesks.Application.Responses.WorkOrder.Category;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.WorkOrder.Category;

public class WorkOrderCategoryPagingRequest : PagedRequest, IRequest<Result<PaginatedResult<WorkOrderCategoryResponse>>>
{
}
