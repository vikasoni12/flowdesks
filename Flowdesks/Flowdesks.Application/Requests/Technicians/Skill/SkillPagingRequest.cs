using Flowdesks.Application.Responses.Technicians.Skill;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Technicians.Skill
{
    public class SkillPagingRequest : PagedRequest, IRequest<Result<PaginatedResult<SkillResponse>>>
    {
        public Guid? TechnicianId { get; set; }
    }
}
