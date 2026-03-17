using Flowdesks.Application.Responses.Asset;
using Flowdesks.Application.Responses.Supplier;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Supplier;

public class SupplierCategoryPagingRequest : PagedRequest, IRequest<Result<PaginatedResult<SupplierCategoryResponse>>>
{
}