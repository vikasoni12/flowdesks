using Flowdesks.Application.Responses.Technicians.Qualification;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Technicians
{
    public class TechnicianQualificationPagingRequest : PagedRequest, IRequest<Result<PaginatedResult<QualificationResponse>>>
    {
        public Guid? TechnicianId { get; set; }
        public bool Expired { get; set; } = false;
    }
}
