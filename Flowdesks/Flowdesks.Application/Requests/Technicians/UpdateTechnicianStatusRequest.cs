
using Flowdesks.Application.Responses.Technicians;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Technicians;

public class UpdateTechnicianStatusRequest : IRequest<Result<TechnicianResponse>>
{
    public Guid Id { get; set; }
    public string Status { get; set; }
}
