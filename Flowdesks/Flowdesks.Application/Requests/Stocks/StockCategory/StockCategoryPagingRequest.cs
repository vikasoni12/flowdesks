using Flowdesks.Application.Responses.Stocks;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Stocks.StockCategory
{
    public class StockCategoryPagingRequest : PagedRequest, IRequest<Result<PaginatedResult<StockCategoryResponse>>>
    {
    }
}
