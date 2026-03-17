using Flowdesks.Application.Features.Site.Query.Export;
using Flowdesks.Application.Interfaces.Identity;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.Site;
using Flowdesks.Application.Specifications.Base;
using static Flowdesks.Shared.Constants.Common.ApplicationConstants;

namespace Flowdesks.Application.Specifications.Site
{
    public class SiteFilterSpecification : Specification<Domain.Entities.Sites.Site>
    {
        private readonly ICurrentUserService _userService;

        public SiteFilterSpecification(SitePagingRequest request, ICurrentUserService userService)
        {
            _userService = userService;

            if (request.UserIds != null && request.UserIds.Count > 0)
            {
                And(p =>  p.Users.Any(z => request.UserIds.Any(y => y == z.Id)));
            }

            if (!String.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.Name.Contains(request.StringSearch)
                || p.Address.Contains(request.StringSearch)
                || p.ContactNumber.Contains(request.StringSearch)
                || p.TelephoneNumber.Contains(request.StringSearch)
                || p.Code.Contains(request.StringSearch)
                || p.SiteCountry.Name.Contains(request.StringSearch)
                || p.PostCode.Contains(request.StringSearch));
            }

            if (request.CountryId != null && request.CountryId.Count > 0)
            {
                And(p => request.CountryId.Contains(p.CountryId.Value));
            }

            if (request.TeamIds != null && request.TeamIds.Any())
            {
                And(p => p.Teams.Any(team => request.TeamIds.Contains(team.Id)));
            }

            And(p => _userService.UserRoles.Any(x => x == RoleConstants.AdministratorRole) || p.Users.Any(x => x.Id.ToString() == userService.UserId));
        }

        public SiteFilterSpecification(ExportSitesAsExcelQuery request, ICurrentUserService userService)
        {
            _userService = userService;

            if (!String.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.Name.Contains(request.StringSearch)
                || p.Address.Contains(request.StringSearch)
                || p.ContactNumber.Contains(request.StringSearch)
                || p.TelephoneNumber.Contains(request.StringSearch)
                || p.Code.Contains(request.StringSearch)
                || p.PostCode.Contains(request.StringSearch));
            }

            if (request.CountryId != null && request.CountryId.Count > 0)
            {
                And(p => request.CountryId.Contains(p.SiteCountry.Id));
            }

            And(p => _userService.UserRoles.Any(x => x == RoleConstants.AdministratorRole) || p.Users.Any(x => x.Id.ToString() == userService.UserId));
        }
    }
}