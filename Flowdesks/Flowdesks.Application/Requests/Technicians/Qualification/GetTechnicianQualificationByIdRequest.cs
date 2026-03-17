using Flowdesks.Application.Responses.Technicians.Qualification;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Technicians.Qualification;

public class GetTechnicianQualificationByIdRequest : IRequest<Result<QualificationResponse>>
{
    public Guid Id { get; set; }
}
