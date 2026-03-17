namespace Flowdesks.Application.Responses.Procedures;

public class ProcedurePagingResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? AssetTypeId { get; set; }
    public string? AssetType { get; set; }
    public string Category { get; set; }
    public string? Frequency { get; set; }
    public int FrequencyOccurrence { get; set; }
    public string? Description { get; set; }
    public virtual ICollection<ProcedureSectionResponse>? Sections { get; set; }
    public virtual ICollection<ProcedureQuestionResponse>? Questions { get; set; }
    public virtual ICollection<ProcedureMappingResponse>? ProcedureMappings { get; set; }
}