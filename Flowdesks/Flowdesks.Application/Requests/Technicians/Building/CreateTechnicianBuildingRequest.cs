using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Technicians.Building
{
    public class CreateTechnicianBuildingRequest : CreateEditRequest<Domain.Entities.Technicians.TechnicianBuilding>, IRequest<Result<int>>
    {
        public List<TechnicianBuildingRequest> TechnicianBuildings { get; set; }
    }
}
