using Flowdesks.Domain.Entities.Procedure;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Procedures;

public class UpdateProcedureMappingRequest : CreateEditRequest<ProcedureMapping>, IRequest<Result<int>>
{
    public Guid Id { get; set; }
    public Guid EntityId { get; set; }
    public string EntityType { get; set; }
    public Guid ProcedureId { get; set; }
    public virtual ICollection<ProcedureResponseRequest>? Responses { get; set; }
}