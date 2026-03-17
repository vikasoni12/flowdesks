using Flowdesks.Application.Features.Technicians.Skill.Command.Delete;
using Flowdesks.Application.Requests.Technicians.Skill;
using Flowdesks.Application.Specifications.Base;

namespace Flowdesks.Application.Specifications.Technicians.Skill
{
    public class SkillTypeFilterSpecification : Specification<Domain.Entities.SystemPreferences.Technician.Skill>
    {
        public SkillTypeFilterSpecification(SkillPagingRequest request)
        {
            if (request.TechnicianId != null)
            {
                And(p => p.Technicians.Count > 0 && p.Technicians.Any(x => x.TechnicianId.Equals(request.TechnicianId)));
            }

            if (!String.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.Name.Contains(request.StringSearch));
            }
        }

        public SkillTypeFilterSpecification(DeleteSkillCommand request)
        {
            if (request.TechnicianId != null)
            {
                And(p => p.Technicians.Count > 0 && p.Technicians.Any(x => x.Id.Equals(request.TechnicianId)));
            }

            if (request.Ids != null && request.Ids.Count > 0)
            {
                And(p => request.Ids.Contains(p.Id));
            }
        }
    }
}
