using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Technicians.Qualification
{
    public class UpdateQualificationRequest : IRequest<Result<int>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? DateObtained { get; set; }
        public string? ExpiryDate { get; set; }
        public Guid? SkillId { get; set; }      
    }
}
