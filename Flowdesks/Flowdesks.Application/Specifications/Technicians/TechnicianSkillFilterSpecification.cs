using Flowdesks.Application.Features.Technicians.Index.Command.Delete;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.Technicians;

namespace Flowdesks.Application.Specifications.Technicians
{
    public class TechnicianSkillFilterSpecification : Specification<TechnicianSkill>
    {
        public TechnicianSkillFilterSpecification(DeleteTechnicianSkillCommand request)
        {
            if (request.Ids != null && request.Ids.Any())
            {
                And(p => request.Ids.Contains(p.Id));
            }

            if (request.TechnicianId != Guid.Empty || request.TechnicianId != null)
            {
                And(p => p.TechnicianId == request.TechnicianId);
            }
        }
    }
}
