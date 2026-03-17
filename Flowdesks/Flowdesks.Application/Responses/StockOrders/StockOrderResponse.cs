

using Flowdesks.Domain.Entities.Stocks;
using Flowdesks.Domain.Entities.SystemPreferences.Stock;

namespace Flowdesks.Application.Responses.StockOrders;

public class StockOrderResponse
{
    public Guid Id { get; set; }
    public Guid? StockId { get; set; }
    public string Stock { get; set; }
    public Guid SupplierId { get; set; }
    public string Supplier { get; set; }
    public Guid? CategoryId { get; set; }
    public string Category { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TotalCost { get; set; }
    public Guid EntityId { get; set; }
    public string EntityType { get; set; } 
}
