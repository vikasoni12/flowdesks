using Flowdesks.Application.Requests.Supplier;
using Flowdesks.Application.Specifications.Base;

namespace Flowdesks.Application.Specifications.Supplier
{
    public class SupplierFilterSpecification : Specification<Domain.Entities.Suppliers.Supplier>
    {
        public SupplierFilterSpecification(SupplierPagingRequest request)
        {
            if (!string.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.Name.Contains(request.StringSearch) ||
                p.Code.Contains(request.StringSearch) ||
                p.Description.Contains(request.StringSearch));
            }

            if (request.Categories != null && request.Categories.Count() > 0)
            {
                And(p => request.Categories.Contains(p.SupplierCategory.Name));
            }

            if (request.Status != null && request.Status.Count() > 0)
            {
                And(p => request.Status.Contains(p.Status));
            }

            Criteria ??= p => true;
        }
    }
}
