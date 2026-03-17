using Flowdesks.Application.Responses.Asset;
using Flowdesks.Application.Responses.Buldings;
using Flowdesks.Application.Responses.Technicians;
using Flowdesks.Application.Responses.WorkOrder.Priority;
using Flowdesks.Shared.Constants;
using Xero.NetStandard.OAuth2.Model.Asset;

namespace Flowdesks.Application.Responses.PPMs
{
    public class PPMResponse
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string TaskId { get; set; }
        public string Problem { get; set; }
        public decimal StockCost { get; set; }
        public decimal LabourCost { get; set; }
        public decimal TotalCost { get; set; }
        public bool IsPermited { get; set; }
        public DateTime NextServiceDate { get; set; }
        public decimal? Period { get; set; }
        public string Frequency { get; set; }
        public int FrequencyOccurrence { get; set; }
        public Guid? ComplianceTypeId { get; set; }
        public string ComplianceType { get; set; }
        public string CostCentre { get; set; }
        public Guid? CostCentreId { get; set; }
        public string CostCode { get; set; }
        public Guid? CostCodeId { get; set; }
        public string? RequestedBy { get; set; }
        public bool IsStatusModified { get; set; }
        public string? CreatedBy { get;set; }
        public Guid PriorityId { get; set; }
        public PriorityResponse Priority {  get; set; }
        public Guid? InstructionId { get; set; }
        public string Instruction { get; set; }
        public Guid AssetId { get; set; }
        public string AssetName { get; set; }
        public AssetResponse Asset { get; set; }
        public Guid BuildingId { get; set; }
        public string BuildingName { get; set; }
        public BuildingResponse Building { get; set; }
        public Guid LocationId { get; set; }
        public string Location { get; set; }
        public Guid? SupplierId { get; set; }
        public string Supplier { get; set; }
        public Guid? TechnicianId { get; set; }
        public TechnicianResponse Technician { get; set; }
        public DateTime CreatedOn { get; set; }

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
                    if (SupplierId == null && TechnicianId == null)
                    {
                        _status = OrderStatus.NotAssign;
                    }
                    else
                    {
                        _status = OrderStatus.Assign;
                    }
                }
                else
                {
                       _status = value;
                }
            }
        }
       
        private string _status { get; set; }
        public ICollection<PPMSkillResponse> Skills { get; set; }
        public ICollection<SuspendedPPMResponse>? SuspendedPPMs { get; set; }
        public ICollection<PPMStatusTrackerResponse>? PPMStatusTracker { get; set; }
        public string? Color { get; set; }

        private int _estimateTime;
        public int EstimateTime
        {
            get { return _estimateTime; }
            set { _estimateTime = value; }
        }

        public int EstimateHours
        {
            get { return _estimateTime / 60; }
        }

        public int EstimateMinutes
        {
            get { return _estimateTime % 60; }
        }

        public Guid? AssignTo => TechnicianId ?? SupplierId;
    }
}