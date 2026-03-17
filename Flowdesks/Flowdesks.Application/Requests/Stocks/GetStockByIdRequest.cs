using Flowdesks.Application.Responses.Stocks;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Stocks
{
    public class GetStockByIdRequest : IRequest<Result<StockResponse>>
    {
        public Guid StockId { get; set; }
    }
}
