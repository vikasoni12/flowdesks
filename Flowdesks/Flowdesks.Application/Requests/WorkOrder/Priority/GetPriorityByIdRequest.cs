using Flowdesks.Application.Responses.WorkOrder.Priority;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.WorkOrder.Priority
{
    public class GetPriorityByIdRequest : IRequest<Result<PriorityResponse>>
    {
        public Guid Id { get; set; }
    }
}
