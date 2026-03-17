using Flowdesks.Domain.Entities.Suppliers;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Supplier;

public class SupplierCategoryRequest : CreateEditRequest<SupplierCategory>, IRequest<Result<int>>
{
    public string Name { get; set; }
}
