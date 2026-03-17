using Flowdesks.Domain.Entities.Teams;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Teams;

public class CreateUpdateTeamRequest : CreateEditRequest<Team>, IRequest<Result<int>>
{
    public string Name { get; set; }
    public string About { get; set; }
    public List<TeamUserRequest> TeamUsers { get; set; }
    public List<TeamSiteRequest> TeamSites { get; set; }
    public List<TeamBuildingRequest>TeamBuildings { get; set; }
}
