using Flowdesks.Shared.Wrapper;
using MediatR;
using System.ComponentModel;

namespace Flowdesks.Application.Requests.WorkOrder;

public class CreateWorkOrderRequest : CreateEditRequest<Flowdesks.Domain.Entities.WorkOrder.WorkOrder>, IRequest<Result<string>>
{
    public string WorkOrderId { get; set; }
    public string Reporter { get; set; }
    public string Email { get; set; }
    public Guid? WorkOrderCategoryId { get; set; }
    public string Phone { get; set; }
    public string Problem { get; set; }
    public Guid? CostCentreId { get; set; }
    public Guid? RequestSourceId { get; set; }
    public Guid? CostCodeId { get; set; }
    public string Respond { get; set; }
    public string Attend { get; set; }
    public string Finished { get; set; }
    public DateTime? RaisedDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public Guid? AssetId { get; set; }
    public Guid? PriorityId { get; set; }
    public Guid? BuildingId { get; set; }
    public Guid? LocationId { get; set; }
    public Guid? AssignedSupplierId { get; set; }
    public Guid? AssignedTechnicianId { get; set; }
    public string? Status { get; set; }
    public string? PermitStatus { get; set; }
    public bool IsStatusModified { get; set; }
    public bool? IsAccepted { get; set; }
    public Guid? AssignedUserId { get; set; }
    public Guid? AssignTo { get; set; }

}
