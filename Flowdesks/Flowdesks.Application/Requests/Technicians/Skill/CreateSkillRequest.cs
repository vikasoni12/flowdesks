using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Technicians.Skill
{
    public class CreateSkillRequest : CreateEditRequest<Domain.Entities.SystemPreferences.Technician.Skill>, IRequest<Result<int>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? TechnicianId {  get; set; }
        public bool? IsNewSkill { get; set; } = true;
    }
}
