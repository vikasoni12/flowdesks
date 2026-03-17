using Flowdesks.Application.Responses.Technicians.Qualification;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Technicians.Qualification
{
    public class QualificationPagingRequest : PagedRequest, IRequest<Result<PaginatedResult<QualificationResponse>>>
    {
        public List<Guid?> SkillIds { get;  set; }
    }
}
