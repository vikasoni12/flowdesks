using Flowdesks.Application.Responses.Stocks;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Stocks
{
    public class UpdateStockRequest : CreateEditRequest<Domain.Entities.Stocks.Stock>, IRequest<Result<StockResponse>>
    {
        public Guid? Id { get; set; }
        public string PartCode { get; set; }
        public string PartName { get; set; }
        public string Manufacturer { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? BuildingId { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? SupplierId { get; set; }
        public string Bin { get; set; }
        public string Description { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? MinQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public UploadByteArray? ProfilePicture { get; set; } = new();
    }
}
