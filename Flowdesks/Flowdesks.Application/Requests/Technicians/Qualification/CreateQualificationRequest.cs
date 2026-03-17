using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Technicians.Qualification
{
    public class CreateQualificationRequest : IRequest<Result<int>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DateObtained { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public Guid? SkillId { get; set; }
        public Guid? TechnicianId { get; set; }
        public bool? IsNew { get; set; }
    }
}
