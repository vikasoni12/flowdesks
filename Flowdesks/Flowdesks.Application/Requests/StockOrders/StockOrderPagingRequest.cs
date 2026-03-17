using Flowdesks.Application.Responses.StockOrders;
using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.StockOrders;

public class StockOrderPagingRequest :PagedRequest, IRequest<Result<PaginatedResult<StockOrderResponse>>>
{
    public EntityType? EntityType { get; set; }
    public string? EntityId { get; set; }

}
