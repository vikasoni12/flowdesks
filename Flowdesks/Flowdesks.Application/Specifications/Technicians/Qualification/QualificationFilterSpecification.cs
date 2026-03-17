using Flowdesks.Application.Features.Technicians.Qualification.Command.Delete;
using Flowdesks.Application.Requests.Technicians.Qualification;
using Flowdesks.Application.Specifications.Base;

namespace Flowdesks.Application.Specifications.Technicians.Qualification
{
    public class QualificationFilterSpecification : Specification<Domain.Entities.SystemPreferences.Technician.Qualification>
    {
        public QualificationFilterSpecification(QualificationPagingRequest request)
        {
            if (!String.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.Name.Contains(request.StringSearch));
            }
        }

        public QualificationFilterSpecification(DeleteQualificationCommand request)
        {
            if (request.Ids != null && request.Ids.Count > 0)
            {
                And(p => request.Ids.Contains(p.Id));
            }
        }
    }
}