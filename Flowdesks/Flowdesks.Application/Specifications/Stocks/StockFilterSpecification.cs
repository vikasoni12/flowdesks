using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.Stocks;
using Flowdesks.Application.Specifications.Base;
using static Flowdesks.Shared.Constants.Common.ApplicationConstants;

namespace Flowdesks.Application.Specifications.Stocks
{
    public class StockFilterSpecification : Specification<Domain.Entities.Stocks.Stock>
    {
        private readonly ICurrentUserService _userService;
        public StockFilterSpecification(StockPagingRequest request, ICurrentUserService userService)
        {
            _userService = userService;

            if (request.CategoryIds != null && request.CategoryIds.Count > 0)
            {
                And(p => p.CategoryId != null && request.CategoryIds.Contains((Guid)p.CategoryId));
            }

            if (request.SupplierIds != null && request.SupplierIds.Count > 0)
            {
                And(p => p.SupplierId != null && request.SupplierIds.Contains((Guid)p.SupplierId));
            }
            if (request.BuildingIds != null && request.BuildingIds.Count > 0)
            {
                And(p => p.BuildingId != null && request.BuildingIds.Contains((Guid)p.BuildingId));
            }

            if (request.BuildingIds != null && request.BuildingIds.Count > 0)
            {
                And(p => p.BuildingId != null && request.BuildingIds.Contains((Guid)p.BuildingId));
            }

            if (!string.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.PartName.Contains(request.StringSearch) ||
                p.PartCode.Contains(request.StringSearch) ||
                p.Building.Name.Contains(request.StringSearch) ||
                p.Location.Name.Contains(request.StringSearch) ||
                p.Supplier.Name.Contains(request.StringSearch) ||
                p.Description.Contains(request.StringSearch));
            }

            And(p => _userService.UserRoles.Any(x => x == RoleConstants.AdministratorRole) || p.Building.Users.Any(x => x.Id.ToString() == userService.UserId));

            Criteria ??= p => true;
        }

        public ICurrentUserService UserService { get; }
    }
}
