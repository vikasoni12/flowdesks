using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Technicians
{
    public  class UpdateTechnicianSkillRequest : IRequest<Result<int>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set;}
    }
}
