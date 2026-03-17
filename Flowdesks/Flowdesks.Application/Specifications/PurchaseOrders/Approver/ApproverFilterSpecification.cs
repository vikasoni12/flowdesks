using Flowdesks.Application.Requests.PurchaseOrders.Approver;
using Flowdesks.Application.Specifications.Base;

namespace Flowdesks.Application.Specifications.PurchaseOrders.Approver;

public class ApproverFilterSpecification : Specification<Domain.Entities.PurchaseOrders.Approver>
{
    public ApproverFilterSpecification(ApproverPagingRequest request)
    {
        if (!string.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.Name.Contains(request.StringSearch) ||
            p.Email.Contains(request.StringSearch));
        }
    }
}