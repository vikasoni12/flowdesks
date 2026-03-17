using Flowdesks.Application.Features.Technicians.Index.Command.Delete;
using Flowdesks.Application.Requests.Technicians;
using Flowdesks.Application.Specifications.Base;

namespace Flowdesks.Application.Specifications.Technicians
{
    public class TechnicianQualificationFilterSpecification : Specification<Domain.Entities.Technicians.TechnicianQualification>
    {
        public TechnicianQualificationFilterSpecification(DeleteTechnicianQualificationCommand request)
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

        public TechnicianQualificationFilterSpecification(TechnicianQualificationPagingRequest request)
        {
            if (request.TechnicianId != null)
            {
                And(x => x.TechnicianId.Equals(request.TechnicianId));
            }

            if (request.Expired)
            {
                And(x => x.ExpiryDate < DateTime.Today.AddDays(7));
            }
        }
    }
}
