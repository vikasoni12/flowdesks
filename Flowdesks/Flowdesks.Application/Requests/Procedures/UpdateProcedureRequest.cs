using Flowdesks.Domain.Entities.Procedure;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Procedures;

public class UpdateProcedureRequest : CreateEditRequest<Procedure>, IRequest<Result<int>>
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public Guid? AssetTypeId { get; set; }
    public string Category { get; set; }
    public string? Frequency { get; set; }
    public int? FrequencyOccurrence { get; set; }
    public string Description { get; set; }
}