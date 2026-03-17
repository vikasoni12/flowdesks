using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests;
using Flowdesks.Application.Requests.Buildings;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.Buildings;
using static Flowdesks.Shared.Constants.Common.ApplicationConstants;

namespace Flowdesks.Application.Specifications.Buildings;

public class BuildingFilterSpecification : Specification<Building>
{
    private readonly ICurrentUserService _userService;

    public BuildingFilterSpecification(BuildingPagingRequest request, ICurrentUserService userService)
    {
        _userService = userService;

        if (request.UserIds != null && request.UserIds.Count > 0)
        {
            And(p => p.Users.Any(z => request.UserIds.Any(y => y == z.Id)));
        }

        if (request.SiteIds != null && request.SiteIds.Count > 0)
        {
            And(p => p.SiteId != null && request.SiteIds.Contains((Guid)p.SiteId));
        }

        if (request.BuildingIds != null && request.BuildingIds.Count > 0)
        {
            And(p => request.BuildingIds.Contains(p.Id));
        }

        if (request.BuildingTypeIds != null && request.BuildingTypeIds.Count > 0)
        {
            And(p => p.BuildingTypeId != null && request.BuildingTypeIds.Contains((Guid)p.BuildingTypeId));
        }

        if (request.OccupancyTypes != null && request.OccupancyTypes.Count > 0)
        {
            And(p => p.OccupancyType != null && request.OccupancyTypes.Contains(p.OccupancyType));
        }

        if (request.TechnicianId != null)
        {
            And(p => p.Technicians.Count > 0 && p.Technicians.Any(x => x.Id.Equals(request.TechnicianId)));
        }

        if (!string.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.Name.Contains(request.StringSearch) ||
            p.Address.Contains(request.StringSearch) ||
            p.Description.Contains(request.StringSearch) ||
            p.OccupancyType.Contains(request.StringSearch));
        }

        And(p => _userService.UserRoles.Any(x => x == RoleConstants.AdministratorRole) || p.Users.Any(x => x.Id.ToString() == userService.UserId));
    }

    public BuildingFilterSpecification(FilterPagedRequest request)
    {
        if (request.SiteIds != null && request.SiteIds.Count > 0)
        {
            And(p => p.SiteId != null && request.SiteIds.Contains((Guid)p.SiteId));
        }

        if (request.BuildingIds != null && request.BuildingIds.Count > 0)
        {
            And(p => request.BuildingIds.Contains(p.Id));
        }
    }
}