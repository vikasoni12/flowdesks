using Flowdesks.Application.Requests.StockOrders;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.StockOrders;
using Flowdesks.Shared.Enums;
using MediatR;

namespace Flowdesks.Application.Specifications.StockOrders;

public class StockOrderFilterSpecification :Specification<StockOrder>
{
    public StockOrderFilterSpecification(StockOrderPagingRequest request)
    {
        if (!string.IsNullOrEmpty(request.EntityId))
        {
            And(p => p.EntityId.ToString().Equals(request.EntityId));
        }

        if (!string.IsNullOrEmpty(Enum.GetName(typeof(EntityType), request.EntityType)))
        {
            var value = Enum.GetName(typeof(EntityType), request.EntityType);
            And(p => p.EntityType.Equals(value));
        }

        if (!String.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.Supplier.Equals(request.StringSearch)
                || p.Stock.PartCode.Equals(request.StringSearch)
                || p.Quantity.ToString().Contains(request.StringSearch));
        }
    }
}
