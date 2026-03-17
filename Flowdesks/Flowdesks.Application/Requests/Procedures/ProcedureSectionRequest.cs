using Flowdesks.Domain.Entities.Procedure;

namespace Flowdesks.Application.Requests.Procedures;

public class ProcedureSectionRequest
{
    public Guid? Id { get; set; }
    public Guid? ProcedureId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<ProcedureQuestionRequest> Questions { get; set; }
}