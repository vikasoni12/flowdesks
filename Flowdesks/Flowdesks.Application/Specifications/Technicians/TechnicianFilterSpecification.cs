using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.Technicians;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Shared.Enums;
using static Flowdesks.Shared.Constants.Common.ApplicationConstants;

namespace Flowdesks.Application.Specifications.Technicians
{
    public class TechnicianFilterSpecification : Specification<Domain.Entities.Technicians.Technician>
    {
        private readonly ICurrentUserService _userService;

        public TechnicianFilterSpecification(TechnicianPagingRequest request, ICurrentUserService userService)
        {
            _userService = userService;

            if (request.SiteIds != null && request.SiteIds.Any())
            {
                And(p => p.Sites.Any(site => request.SiteIds.Contains(site.Id)));
            }

            if (request.SkillIds != null && request.SkillIds.Any())
            {
                And(p => p.Skills.Any(skill => request.SkillIds.Contains(skill.SkillId)));
                //if (request.QualificationIds != null && request.QualificationIds.Any())
                //{
                //    And(p => p.Qualifications.Any(qualification => request.QualificationIds.Contains(qualification.Id)));
                //}
            }
            if (request.SupplierIds != null && request.SupplierIds.Any())
            {
                And(p => p.SupplierId != null && request.SupplierIds.Contains((Guid)p.SupplierId));

                if (request.IsExternal == false)
                {
                    And(p => p.IsExternalResource == !request.IsExternal);
                }
            }

            if (request.BuildingId != null)
            {
                And(p => p.Buildings.Any(x => x.Id == request.BuildingId));
            }

            if (request.BuildingIds != null && request.BuildingIds.Any())
            {
                And(p => p.Buildings.Any(building => request.BuildingIds.Contains(building.Id)));
            }
            if (request.QualificationIds != null && request.QualificationIds.Any())
            {
                And(p => p.Qualifications.Any(qualification => request.QualificationIds.Contains(qualification.QualificationId)));
            }
            if (!string.IsNullOrEmpty(request.Status))
            {
                And(p=>p.Status.Equals(request.Status));
            }

            if (!string.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.Name.Contains(request.StringSearch) ||
                         p.IdNumber.Contains(request.StringSearch) ||
                         p.Sites.Any(site => site.Name.Contains(request.StringSearch)) ||
                         p.Buildings.Any(building => building.Name.Contains(request.StringSearch)) ||
                         p.Email.Contains(request.StringSearch) ||
                         p.MobileNumber.Contains(request.StringSearch));
            }

            And(p => _userService.UserRoles.Any(x => x == RoleConstants.AdministratorRole) || (p.Buildings.Any(x => x.Users.Any(x => x.Id.ToString() == userService.UserId)) && p.Sites.Any(x => x.Users.Any(x => x.Id.ToString() == userService.UserId))));
        }
    }
}