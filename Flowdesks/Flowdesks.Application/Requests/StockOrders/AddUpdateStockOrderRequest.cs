using Flowdesks.Domain.Entities.StockOrders;
using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Wrapper;
using MediatR;


namespace Flowdesks.Application.Requests.StockOrders;

public class AddUpdateStockOrderRequest : CreateEditRequest<StockOrder>, IRequest<Result<int>>
{
    public Guid? StockId { get; set; }
    public Guid SupplierId { get; set; }
    public decimal Quantity { get; set; }
    public int? StockQuantity { get; set; }
    public Guid EntityId { get; set; }
    public EntityType EntityType { get; set; } //EntityType Enum
}