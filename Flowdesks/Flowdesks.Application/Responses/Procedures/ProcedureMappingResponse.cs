namespace Flowdesks.Application.Responses.Procedures;

public class ProcedureMappingResponse
{
    public Guid Id { get; set; }
    public Guid EntityId { get; set; }
    public string EntityType { get; set; }
    public Guid ProcedureId { get; set; }

    public string? EntityCode { get; set; }
    public string? Problem {  get; set; }
    public string? TechnicianName {  get; set; }

    public virtual ICollection<ProcedureResponse>? Responses { get; set; }
}