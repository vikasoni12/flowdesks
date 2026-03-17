using Flowdesks.Domain.Entities.Buildings;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Buildings;

public class AddUpdateLocationRequest : CreateEditRequest<BuildingLocation>, IRequest<Result<int>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string? Floor { get; set; }
    public Guid BuildingId { get; set; }
}
