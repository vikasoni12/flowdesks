using Flowdesks.Application.Responses.Contacts;
using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Contacts;

public class ContactPagingRequest : PagedRequest, IRequest<Result<PaginatedResult<ContactResponse>>>
{
    public EntityType? EntityType { get; set; }
    public string? EntityId { get; set; }
    public string CreatedBy { get; set; }
}