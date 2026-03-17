using Flowdesks.Application.Responses.Technicians;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Technicians
{
    public class GetTechnicianByIdRequest : IRequest<Result<TechnicianResponse>>
    {
        public Guid Id { get; set; }
    }
}
