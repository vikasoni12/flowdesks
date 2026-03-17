using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Technicians;

public class UpdateTechnicianQualificationRequest : IRequest<Result<int>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid SkillId { get; set; }
    public Guid TechnicianId { get; set; }
    public DateTime DateObtained { get; set; }
    public DateTime ExpiryDate { get; set; }
}
