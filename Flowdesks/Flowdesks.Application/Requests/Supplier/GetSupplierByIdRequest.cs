using Flowdesks.Application.Responses.Supplier;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Supplier
{
    public class GetSupplierByIdRequest : IRequest<Result<SupplierResponse>>
    {
        public Guid? SupplierId { get; set; }
    }
}
