using Flowdesks.Application.Responses.Supplier;
using Flowdesks.Application.Responses.Technicians;
using Flowdesks.Application.Responses.WorkOrder.Priority;
using Flowdesks.Shared.Constants;

namespace Flowdesks.Application.Responses.WorkOrder
{
    public class WorkOrderResponse
    {
        public Guid Id { get; set; }
        public string WorkOrderId { get; set; }
        public string Reporter { get; set; }
        public string Email { get; set; }
        public Guid? WorkOrderCategoryId { get; set; }
        public string Category { get; set; }
        public string Phone { get; set; }
        public string Problem { get; set; }
        public string CostCentre { get; set; }
        public Guid? CostCentreId { get; set; }
        public string CostCode { get; set; }
        public Guid? CostCodeId { get; set; }
        public Guid? RequestSourceId { get; set; }
        public string Source { get; set; }
        public string Respond { get; set; }
        public string Attend { get; set; }
        public string Finished { get; set; }
        public string? RequestedBy { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? RaisedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public Guid? AssetId { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public Guid? PriorityId { get; set; }
        public PriorityResponse Priority { get; set; }
        public string PriorityName { get; set; }
        public Guid? BuildingId { get; set; }
        public string BuildingName { get; set; }
        public Guid? LocationId { get; set; }
        public string BuildingLocationName { get; set; }
        public Guid? AssignedSupplierId { get; set; }
        public SupplierResponse Supplier { get; set; }
        public Guid? AssignedTechnicianId { get; set; }
        public TechnicianResponse Technician { get; set; }
        public decimal? EstimateParts { get; set; }
        public decimal? EstimateLabour { get; set; }
        public decimal? EstimateTotal { get; set; }
        public decimal? ActualParts { get; set; }
        public decimal? ActualLabour { get; set; }
        public decimal? ActualTotal { get; set; }
        public string? PermitStatus { get; set; }
        public Guid? SkillId { get; set; }
        public Guid? QualificationId { get; set; }
        public bool IsStatusModified { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (!IsStatusModified)
                {
                    if (AssignedSupplierId != null || AssignedTechnicianId != null)
                    {
                        _status = OrderStatus.Assign;
                    }
                    else
                    {
                        _status = OrderStatus.NotAssign;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Attend) && !IsStatusModified)
                    {
                        _status = OrderStatus.InProgress;
                    }
                    else
                    {
                        _status = value;

                    }
                }
            }
        }

        public string? Color
        {
            get
            {
                if (DueDate.HasValue)
                {
                    TimeSpan timeDifference = DueDate.Value - DateTime.UtcNow;
                    int minutesDifference = (int)timeDifference.TotalMinutes;
                    return minutesDifference < 0 ? "red" : "";
                }
                else
                {
                    return "";
                }
            }
        }

        public Guid? PermitId { get; set; }
        public string? Permit { get; set; }

        private string _status { get; set; }
        public bool IsHistoricalWorkOrder { get; set; }
        public bool? IsAccepted { get; set; }
        public Guid? AssignedUserId { get; set; }
    }
}

