using Flowdesks.Application.Responses.Documents;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Documents
{
    public class DocumentCategoryPagingRequest : PagedRequest, IRequest<Result<PaginatedResult<DocumentCategoryResponse>>>
    {
    }
}
