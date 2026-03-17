using Flowdesks.Application.Requests;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Responses.Identity
{
    public class GridArgument : PagedRequest, IRequest<PaginatedResult<UserGridResponse>>
    {
        public string SearchTerm { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int TotalRecords { get; set; }
        public string Status { get; set; }
    }
}
