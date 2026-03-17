using Flowdesks.Application.Responses.PurchaseOrders.Approver;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.PurchaseOrders.Approver;

public class ApproverPagingRequest : FilterPagedRequest, IRequest<Result<PaginatedResult<ApproverResponse>>>
{
}