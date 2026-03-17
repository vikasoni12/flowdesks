using Flowdesks.Application.Responses.Technicians.Skill;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Technicians
{
    public class TechnicianSkillPagingRequest : PagedRequest, IRequest<Result<PaginatedResult<SkillResponse>>>
    {
        public Guid? TechnicianId { get; set; }
    }
}
