using Flowdesks.Application.Responses.PPMs;

namespace Flowdesks.Application.Responses.WorkOrder;

public class CombinedPPMAndWorkOrderResponse
{
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
    public string TaskId { get; set; }
    public string Type { get; set; }
    public string Building { get; set; }
    public string Asset { get; set; }
    public string Technician { get; set; }
    public string Supplier { get; set; }
    public string Status { get; set; }
    public DateTime ServicedDate { get; set; }
    public WorkOrderResponse? WorkOrder { get; set; }
    public PPMResponse? PPM { get; set; }
}

public class ExportPPMAndWorkOrderResponse
{
    public string TaskId { get; set; }
    public string Type { get; set; }
    public string Building { get; set; }
    public string Asset { get; set; }
    public string Technician { get; set; }
    public string Supplier { get; set; }
    public string Status { get; set; }
    public DateTime ServicedDate { get; set; }
}