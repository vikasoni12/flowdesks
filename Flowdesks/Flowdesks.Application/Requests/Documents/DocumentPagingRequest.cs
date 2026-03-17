using Flowdesks.Application.Responses.Documents;
using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Documents;

public class DocumentPagingRequest : PagedRequest, IRequest<Result<PaginatedResult<DocumentResponse>>>
{
    public EntityType? EntityType { get; set; }
    public string? EntityId { get; set; }
    public string? CreatedBy { get; set; }
    public string? DocumentType { get; set; }
}