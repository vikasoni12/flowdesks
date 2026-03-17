using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Procedures;

public class UpdateProcedureQuestionRequest : IRequest<Result<int>>
{
    public Guid ProcedureId { get; set; }
    public virtual ICollection<ProcedureSectionRequest>? Sections { get; set; }
    public virtual ICollection<ProcedureQuestionRequest> Questions { get; set; }
}