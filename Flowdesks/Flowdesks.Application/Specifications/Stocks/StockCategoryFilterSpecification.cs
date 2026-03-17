using Flowdesks.Application.Requests.Stocks.StockCategory;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.SystemPreferences.Stock;

namespace Flowdesks.Application.Specifications.Stocks
{
    public class StockCategoryFilterSpecification : Specification<StockCategory>
    {
        public StockCategoryFilterSpecification(StockCategoryPagingRequest request)
        {
            if (!string.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.Name.Contains(request.StringSearch));
            }
        }
    }
}

