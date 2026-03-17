using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Stocks.StockCategory
{
    public class AddUpdateStockCategoryRequest : IRequest<Result<int>>
    {
        public string Name { get; set; }
    }
}
