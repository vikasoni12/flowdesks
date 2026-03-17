using Flowdesks.Domain.Entities.WorkRequests;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.WorkRequests;

public class CreateWorkRequest : CreateEditRequest<WorkRequest>, IRequest<Result<int>>
{
    public string Reporter { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Problem { get; set; }
    public string Description { get; set; }
    public string DocumentUrl { get; set; }
    public string DocumentName { get; set; }
    public string Status { get; set; }
    public Guid BuildingId { get; set; }
    public Guid LocationId { get; set; }
}