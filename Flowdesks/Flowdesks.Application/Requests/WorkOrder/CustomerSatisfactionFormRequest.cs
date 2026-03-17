using Flowdesks.Domain.Entities.WorkOrder;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.WorkOrder;

public class CustomerSatisfactionFormRequest : CreateEditRequest<CustomerSatisfactionForm>, IRequest<Result<int>>
{
    public Guid Id { get; set; }
    public Guid WorkOrderId { get; set; }
    public string Response { get; set; }
    public string Comment { get; set; }
    public Guid? SendBy { get; set; }
}