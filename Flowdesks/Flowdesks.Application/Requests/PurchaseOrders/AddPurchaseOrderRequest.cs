using Flowdesks.Application.Attributes;
using Flowdesks.Domain.Entities.PurchaseOrders;
using Flowdesks.Shared.Wrapper;
using MediatR;
using System.ComponentModel;

namespace Flowdesks.Application.Requests
{
    public class AddPurchaseOrderRequest : CreateEditRequest<PurchaseOrder>, IRequest<Result<int>>
    {
        [Description("Category")]
        public Guid? CategoryId { get; set; }

        [Description("Supplier")]
        [IsRequiredField(true)]
        public Guid? SupplierId { get; set; }

        [Description("Part code")]
        [IsRequiredField(true)]
        public Guid? PartCodeId { get; set; }

        [Description("Unit quantity")]
        [IsRequiredField(true)]
        public int? UnitQuantity { get; set; }

        [Description("Unit cost")]
        public decimal? UnitCost { get; set; }

        [Description("Total cost")]
        [IgnoreField(true)]
        public decimal? TotalCost { get; set; }

        [Description("Delivery Point")]
        public string? DeliveryPoint { get; set; }
        public string? Description { get; set; }

        [Description("Approver")]
        public Guid? ApproverId { get; set; }
        public string Status { get; set; }
    }
}
