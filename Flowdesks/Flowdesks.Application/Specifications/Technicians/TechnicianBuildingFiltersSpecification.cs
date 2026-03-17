using Flowdesks.Application.Features.Technicians.Index.Command.Delete;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.Technicians;

namespace Flowdesks.Application.Specifications.Technicians
{
    public class TechnicianBuildingFiltersSpecification : Specification<TechnicianBuilding>
    {
        public TechnicianBuildingFiltersSpecification(DeleteTechnicianBuildingCommand request)
        {
            if (request.Ids != null && request.Ids.Any())
            {
                And(p => request.Ids.Contains(p.BuildingId));
            }

            if (request.TechnicianId != Guid.Empty || request.TechnicianId != null)
            {
                And(p => p.TechnicianId == request.TechnicianId);
            }
        }
    }
}
