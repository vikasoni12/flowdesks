using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.WorkRequests;

public class AddUpdateSelfServiceUpdateRequest : IRequest<Result<int>>
{
    public Guid? Id { get; set; }
    public Guid SiteId { get; set; }
    public Guid BuildingId { get; set; }
    public string Comment { get; set; }
}