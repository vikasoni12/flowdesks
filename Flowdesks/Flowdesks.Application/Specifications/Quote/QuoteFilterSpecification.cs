using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.Quotes;
using Flowdesks.Application.Specifications.Base;
using static Flowdesks.Shared.Constants.Common.ApplicationConstants;

namespace Flowdesks.Application.Specifications.Quote;

public class QuoteFilterSpecification : Specification<Domain.Entities.Quotes.Quote>
{
    private readonly ICurrentUserService _userService;

    public QuoteFilterSpecification(QuotePagingRequest request, ICurrentUserService userService)
    {
        _userService = userService;

        if (request.BuildingIds != null && request.BuildingIds.Count > 0)
        {
            And(p => p.BuildingId != null && request.BuildingIds.Contains((Guid)p.BuildingId));
        }

        if (request.CategoryIds != null && request.CategoryIds.Count > 0)
        {
            And(p => p.WorkOrderCategoryId != null && request.CategoryIds.Contains((Guid)p.WorkOrderCategoryId));
        }

        if (request.Status != null && request.Status.Count > 0)
        {
            And(p => p.Status != null && request.Status.Contains(p.Status));
        }
        if (!string.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.Building.Name.Contains(request.StringSearch) ||
            p.QuoteNumber.Contains(request.StringSearch) ||
            p.Category.Name.Contains(request.StringSearch) ||
            p.JobType.Contains(request.StringSearch) ||
            p.JobDetails.Contains(request.StringSearch));
        }

        And(p => _userService.UserRoles.Any(x => x == RoleConstants.AdministratorRole) || p.Building.Users.Any(x => x.Id.ToString() == userService.UserId));
    }
}
