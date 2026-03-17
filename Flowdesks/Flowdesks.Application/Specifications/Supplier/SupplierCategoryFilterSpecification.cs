using Flowdesks.Application.Requests.Supplier;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.Suppliers;

namespace Flowdesks.Application.Specifications.Supplier;

public class SupplierCategoryFilterSpecification : Specification<SupplierCategory>
{
    public SupplierCategoryFilterSpecification(SupplierCategoryPagingRequest request)
    {
        if (!string.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.Name.Contains(request.StringSearch));
        }
    }
}