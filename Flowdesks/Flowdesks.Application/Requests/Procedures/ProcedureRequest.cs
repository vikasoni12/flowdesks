using Flowdesks.Domain.Entities.Procedure;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Procedures;

public class ProcedureRequest : CreateEditRequest<Procedure>, IRequest<Result<int>>
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public Guid? AssetTypeId { get; set; }
    public string Category { get; set; }
    public string? Frequency { get; set; }
    public int? FrequencyOccurrence { get; set; }
    public string? Description { get; set; }
    public virtual ICollection<ProcedureSectionRequest>? Sections { get; set; }
    public virtual ICollection<ProcedureQuestionRequest> Questions { get; set; }
    public virtual ICollection<ProcedureMappingRequest>? ProcedureMappings { get; set; }
}