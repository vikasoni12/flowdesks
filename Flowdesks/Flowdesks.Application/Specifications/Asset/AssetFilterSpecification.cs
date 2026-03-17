using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests;
using Flowdesks.Application.Requests.Asset;
using Flowdesks.Application.Requests.WorkOrder;
using Flowdesks.Application.Specifications.Base;
using static Flowdesks.Shared.Constants.Common.ApplicationConstants;

namespace Flowdesks.Application.Specifications.Asset
{
    public class AssetFilterSpecification : Specification<Domain.Entities.Assets.Asset>
    {
        private readonly ICurrentUserService _userService;

        public AssetFilterSpecification(AssetPagingRequest request, ICurrentUserService userService)
        {
            _userService = userService;

            if (request.SiteIds != null && request.SiteIds.Count > 0)
            {
                And(p => p.SiteId != null && request.SiteIds.Contains((Guid)p.SiteId));
            }

            if (request.BuildingIds != null && request.BuildingIds.Count > 0)
            {
                And(p => p.BuildingId != null && request.BuildingIds.Contains((Guid)p.BuildingId));
            }

            if (request.LocationIds != null && request.LocationIds.Count > 0)
            {
                And(p => p.LocationId != null && request.LocationIds.Contains((Guid)p.LocationId));
            }

            if (request.TypeIds != null && request.TypeIds.Count > 0)
            {
                And(p => p.TypeId != null && request.TypeIds.Contains((Guid)p.TypeId));
            }
            if (request.SupplierIds != null && request.SupplierIds.Count > 0)
            {
                And(p => p.SupplierId != null && request.SupplierIds.Contains((Guid)p.SupplierId));
            }
            if (request.ConditionIds != null && request.ConditionIds.Count > 0)
            {
                And(p => p.HealthAndFinanceDetail.ConditionId != null && request.ConditionIds.Contains((Guid)p.HealthAndFinanceDetail.ConditionId));
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                And(p => p.Status.Equals(request.Status));
            }

            if (!string.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.Name.Contains(request.StringSearch) ||
                p.Code.Contains(request.StringSearch) ||
                p.Site.Name.Contains(request.StringSearch) ||
                p.Building.Name.Contains(request.StringSearch) ||
                p.Location.Name.Contains(request.StringSearch) ||
                p.Description.Contains(request.StringSearch));
            }

            if (request.From != null && request.IsHealthRequired)
            {
                And(p => p.HealthAndFinanceDetail != null
                && p.HealthAndFinanceDetail.PurchaseDate != null
                && p.HealthAndFinanceDetail.PurchaseDate.Value.AddYears((int)p.HealthAndFinanceDetail.LifeSpan.Value) >= request.From);
            }

            if (request.To != null && request.IsHealthRequired)
            {
                And(p => p.HealthAndFinanceDetail != null
                && p.HealthAndFinanceDetail.PurchaseDate != null
                && p.HealthAndFinanceDetail.PurchaseDate.Value.AddYears((int)p.HealthAndFinanceDetail.LifeSpan.Value) <= request.To);
            }

            And(p => _userService.UserRoles.Any(x => x == RoleConstants.AdministratorRole) || (p.Building.Users.Any(x => x.Id.ToString() == userService.UserId) && p.Site.Users.Any(x => x.Id.ToString() == userService.UserId)));
        }

        public AssetFilterSpecification(CustomFilterRequest request)
        {
            if (request.BuildingIds != null && request.BuildingIds.Count > 0)
            {
                And(p => p.BuildingId != null && request.BuildingIds.Contains((Guid)p.BuildingId));
            }

            if (request.SiteIds != null && request.SiteIds.Count > 0)
            {
                And(p => p.BuildingId != null && p.Building.SiteId != null && request.SiteIds.Contains((Guid)p.Building.SiteId));
            }

            if (request.From != null)
            {
                And(p => p.HealthAndFinanceDetail.WarrantyExpiresDate >= request.From.Value.Date);
            }

            if (request.To != null)
            {
                And(p => p.HealthAndFinanceDetail.WarrantyExpiresDate <= request.To.Value.Date);
            }
        }
    }

    public class AssetHealthAndFinanceSpecification : Specification<Domain.Entities.Assets.AssetHealthAndFinanceDetail>
    {
        public AssetHealthAndFinanceSpecification(FilterPagedRequest request)
        {
            if (request.BuildingIds != null && request.BuildingIds.Count > 0)
            {
                And(p => p.Asset.BuildingId != null && request.BuildingIds.Contains((Guid)p.Asset.BuildingId));
            }

            if (request.SiteIds != null && request.SiteIds.Count > 0)
            {
                And(p => p.Asset.SiteId != null && p.Asset.SiteId != null && request.SiteIds.Contains((Guid)p.Asset.SiteId));
            }

            if (request.From != null && request.IsHealthRequired)
            {
                And(p => p.PurchaseDate != null && p.PurchaseDate.Value.AddYears((int)p.LifeSpan.Value) >= request.From);
            }

            if (request.To != null && request.IsHealthRequired)
            {
                And(p => p.PurchaseDate != null && p.PurchaseDate.Value.AddYears((int)p.LifeSpan.Value) <= request.To);
            }
        }
    }
}