using Flowdesks.Domain.Entities.WorkOrder;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.WorkOrder;

public class WorkOrderProcedureRequest : CreateEditRequest<WorkOrderProcedure>, IRequest<Result<int>>
{
    public Guid WorkOrderId { get; set; }
    public string Name { get; set; }
}