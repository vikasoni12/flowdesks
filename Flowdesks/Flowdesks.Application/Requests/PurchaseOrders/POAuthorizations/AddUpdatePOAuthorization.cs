using Flowdesks.Domain.Entities.PurchaseOrders;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.PurchaseOrders.POAuthorizations
{
    public class AddUpdatePOAuthorization : CreateEditRequest<POAuthorization>, IRequest<Result<int>>
    {
        public Guid? Id { get; set; }
        public int AuthorizationLimit { get; set; }
        public bool IsAuthorized { get; set; }
        public bool IsSelfAuthorize { get; set; }
    }
}
