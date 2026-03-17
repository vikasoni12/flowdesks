using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.PurchaseOrders.Approver;

public class AddApproverRequest : CreateEditRequest<Domain.Entities.PurchaseOrders.Approver>, IRequest<Result<int>>
{
    public string Name { get; set; }
    public string Email { get; set; }
}
