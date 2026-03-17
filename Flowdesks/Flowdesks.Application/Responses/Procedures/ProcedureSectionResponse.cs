using Flowdesks.Application.Requests.Procedures;

namespace Flowdesks.Application.Responses.Procedures;

public class ProcedureSectionResponse
{
    public Guid Id { get; set; }
    public Guid ProcedureId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<ProcedureQuestionRequest> Questions { get; set; }
}