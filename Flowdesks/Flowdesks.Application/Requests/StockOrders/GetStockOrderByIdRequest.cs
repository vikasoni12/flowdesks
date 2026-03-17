using Flowdesks.Application.Responses.StockOrders;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.StockOrders
{
    public class GetStockOrderByIdRequest : IRequest<Result<StockOrderResponse>>
    {
        public Guid Id { get; set; }
    }
}
