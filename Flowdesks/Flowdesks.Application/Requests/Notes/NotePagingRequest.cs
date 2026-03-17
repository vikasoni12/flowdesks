using Flowdesks.Application.Responses.Notes;
using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Notes;

public class NotePagingRequest : PagedRequest, IRequest<Result<PaginatedResult<NoteResponse>>>
{
    public Guid? UserId { get; set; }
    public string EntityId { get; set; }
    public EntityType EntityType { get; set; }
    public string CreatedBy { get; set; }
}