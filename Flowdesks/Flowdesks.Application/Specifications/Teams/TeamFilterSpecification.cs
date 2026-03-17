using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.Teams;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.Teams;
using static Flowdesks.Shared.Constants.Common.ApplicationConstants;

namespace Flowdesks.Application.Specifications.Teams;

public class TeamFilterSpecification :Specification<Team>
{
    private readonly ICurrentUserService _userService;

    public TeamFilterSpecification(TeamPagingRequest request, ICurrentUserService userService)
    {
        _userService = userService;

        if (request.SiteIds != null && request.SiteIds.Any())
        {
            And(p => p.Sites.Any(site => request.SiteIds.Contains(site.Id)));
        }

        if (request.BuildingId != null)
        {
            And(p => p.Buildings.Any(x => x.Id == request.BuildingId));
        }
        
        if (request.UserIds != null)
        {
            And(p => p.Users.Any(user=>request.UserIds.Contains(user.Id)));
        }

        if (!string.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.Name.Contains(request.StringSearch) ||
                     p.About.Contains(request.StringSearch) ||
                     p.Sites.Any(site => site.Name.Contains(request.StringSearch)) ||
                     p.Buildings.Any(building => building.Name.Contains(request.StringSearch)) ||
                     p.Users.Any(user =>user.FirstName.Contains(request.StringSearch) || user.LastName.Contains(request.StringSearch)
                     ));
        }

        And(p => _userService.UserRoles.Any(x => x == RoleConstants.AdministratorRole) || p.Users.Any(x => x.Id.ToString() == userService.UserId));
    }
}