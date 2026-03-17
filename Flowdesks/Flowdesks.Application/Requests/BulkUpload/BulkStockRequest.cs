using Flowdesks.Application.Attributes;
using Flowdesks.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowdesks.Application.Requests.BulkUpload;
public class BulkStockRequest : CreateEditRequest<Domain.Entities.Stocks.Stock>, IRequest<Result<int>>
{
    [Description("Part code")]
    [IsRequiredField(true)]
    public string PartCode { get; set; }
    [Description("Part name")]
    [IsRequiredField(true)]
    public string PartName { get; set; }
    public string Manufacturer { get; set; }
    [Description("Category")]
    public Guid? CategoryId { get; set; }
    [Description("Building")]
    public Guid? BuildingId { get; set; }
    [Description("Location")]
    public Guid? LocationId { get; set; }
    [Description("Supplier")]
    [IsRequiredField(true)]
    public Guid? SupplierId { get; set; }
    public string Bin { get; set; }
    public string Description { get; set; }
    [IsRequiredField(true)]
    public decimal? Quantity { get; set; }
    [Description("Unit cost")]
    [IsRequiredField(true)]
    public decimal? UnitCost { get; set; }
    [Description("Min quantity")]
    public decimal? MinQuantity { get; set; }
    [IgnoreField(true)]
    public UploadByteArray? ProfilePicture { get; set; } = new();
}
