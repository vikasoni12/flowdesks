using Flowdesks.Application.Responses.WorkOrder;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.WorkOrder
{
    public class GetWorkOrderByIdRequest : IRequest<Result<WorkOrderResponse>>
    {
        public Guid WorkOrderId { get; set; }
    }
}
