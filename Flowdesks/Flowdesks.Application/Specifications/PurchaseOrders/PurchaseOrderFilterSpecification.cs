using Flowdesks.Application.Requests.PurchaseOrders;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.PurchaseOrders;

namespace Flowdesks.Application.Specifications.PurchaseOrders
{
    public class PurchaseOrderFilterSpecification :Specification<PurchaseOrder>
    {
        public PurchaseOrderFilterSpecification(PurchaseOrderPagingRequest request)
        {
            And(x => !x.IsDeleted);
            if (request.CategoryIds != null && request.CategoryIds.Count > 0)
            {
                And(p => p.CategoryId != null && request.CategoryIds.Contains((Guid)p.CategoryId));
            }
            if (request.SupplierIds != null && request.SupplierIds.Count > 0)
            {
                And(p => p.SupplierId != null && request.SupplierIds.Contains((Guid)p.SupplierId));
            }
            if (request.Status != null && request.Status.Count > 0)
            {
                And(p => p.Status != null && request.Status.Contains(p.Status));
            }

            if (!string.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.PONumber.Contains(request.StringSearch) ||
                p.Category.Name.Contains(request.StringSearch)||
                p.Stock.PartName.Contains(request.StringSearch) ||
                p.Supplier.Name.Contains(request.StringSearch) ||
                p.UnitQuantity.ToString().Contains(request.StringSearch) ||
                p.UnitCost.ToString().Contains(request.StringSearch) ||
                p.CreatedOn.ToString().Contains(request.StringSearch));
            }
        }
    }
}