using Flowdesks.Application.Features.WorkOrder.Index.Query.GetAll;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.WorkOrder;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Shared.Constants;
using Flowdesks.Shared.Enums;
using static Flowdesks.Shared.Constants.Common.ApplicationConstants;

namespace Flowdesks.Application.Specifications.WorkOrder;

public class WorkOrderFilterSpecification : Specification<Domain.Entities.WorkOrder.WorkOrder>
{
    private readonly ICurrentUserService _userService;

    public WorkOrderFilterSpecification(WorkOrderPagingRequest request, ICurrentUserService userService)
    {
        _userService = userService;

        And(x => !x.IsDisabled);

        if (request.BuildingIds != null && request.BuildingIds.Count > 0)
        {
            And(p => p.BuildingId != null && request.BuildingIds.Contains((Guid)p.BuildingId));
        }

        if (request.Status != null && request.Status.Count > 0)
        {
            And(p => p.Status != null && request.Status.Contains(p.Status) && !p.IsHistoricalWorkOrder);
        }

        if (request.IsFromHistory != null)
        {

            if ((bool)request.IsFromHistory)
            {
                And(p => p.Status != null && p.Status.Equals(OrderStatus.ApprovedForPayment) && p.IsHistoricalWorkOrder);
            }
            else
            {
                And(p => !p.IsHistoricalWorkOrder);
            }
        }
        else
        {
            And(p => p.Status != null && p.Status.Equals(OrderStatus.ApprovedForPayment));
        }

        if (request.AssetIds != null && request.AssetIds.Count > 0)
        {
            And(p => p.AssetId != null && request.AssetIds.Contains((Guid)p.AssetId));
        }

        if (request.PriorityIds != null && request.PriorityIds.Count > 0)
        {
            And(p => p.PriorityId != null && request.PriorityIds.Contains((Guid)p.PriorityId));
        }

        if (request.CategoryIds != null && request.CategoryIds.Count > 0)
        {
            And(p => p.WorkOrderCategoryId != null && request.CategoryIds.Contains((Guid)p.WorkOrderCategoryId));
        }

        if (request.SupplierIds != null && request.SupplierIds.Count > 0)
        {
            And(p => p.AssignedSupplierId != null && request.SupplierIds.Contains((Guid)p.AssignedSupplierId));
        }

        if (request.ShowPastDue != null && request.ShowPastDue == true)
        {
            And(p => p.DueDate != null && p.DueDate < DateTime.UtcNow && p.Status != Status.Completed.ToString());
        }

        if (request.IsAccepted != null)
        {
            And(p => p.IsAccepted != null && p.IsAccepted == request.IsAccepted);
        }

        if (request.AssignedUserIds != null && request.AssignedUserIds.Count > 0)
        {
            And(p => p.AssignedUserId != null && request.AssignedUserIds.Contains((Guid)p.AssignedUserId));
        }

        if (!string.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.WorkOrderId.Contains(request.StringSearch) ||
            p.Reporter.Contains(request.StringSearch) ||
            p.Email.Contains(request.StringSearch) ||
            p.Category.Name.Contains(request.StringSearch) ||
            p.Phone.Contains(request.StringSearch));
        }

        if (!string.IsNullOrEmpty(request.UserId.ToString()))
        {
            var isAdmin = userService.UserRoles.Any(role => role == RoleConstants.AdministratorRole);
            if (request.DueDate != null)
            {
                if (isAdmin)
                    And(p => p.DueDate != null && p.DueDate.Value.Date == DateTime.UtcNow.Date);
                else
                    And(p => p.DueDate != null && p.DueDate.Value.Date == DateTime.UtcNow.Date &&
                             (p.AssignedSupplierId == request.UserId || p.AssignedTechnicianId == request.UserId));
            }
            else
            {
                if (!isAdmin)
                    And(p => p.IsAccepted == null &&
                             (p.AssignedSupplierId == request.UserId || p.AssignedTechnicianId == request.UserId));
            }
        }

        if (request.CalendarDate != null)
        {
            And(p => p.DueDate != null && p.DueDate.Value.Date == request.CalendarDate.Value.Date);
        }

        And(p => _userService.UserRoles.Any(x => x == RoleConstants.AdministratorRole) || p.Building.Users.Any(x => x.Id.ToString() == userService.UserId));

    }

    public WorkOrderFilterSpecification(CustomFilterRequest request)
    {
        And(x => !x.IsDisabled);

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
            And(p => p.DueDate.Value.Date >= request.From.Value.Date);
        }

        if (request.To != null)
        {
            And(p => p.DueDate.Value.Date <= request.To.Value.Date);
        }

        if (request.IsForApprovedForPayment)
        {
            And(p => p.Status != null && p.Status.Equals(OrderStatus.ApprovedForPayment));
        }

        if ((bool)request.IsFromHistory)
        {
            And(p => p.Status != null && p.Status.Equals(OrderStatus.ApprovedForPayment) && p.IsHistoricalWorkOrder);
        }
    }

    public WorkOrderFilterSpecification(CombinedPPMWorkOrderRequest request, ICurrentUserService userService)
    {
        And(x => !x.IsDisabled);

        if (request.AssetId.HasValue)
        {
            And(w => w.AssetId == request.AssetId.Value);
        }

        And(w => w.IsHistoricalWorkOrder);
        And(w => w.Status == OrderStatus.ApprovedForPayment);

        And(w => userService.UserRoles.Any(x => x == RoleConstants.AdministratorRole) ||
                 w.Building.Users.Any(x => x.Id.ToString() == userService.UserId));

        AddInclude(x => x.Asset);
        AddInclude(x => x.Supplier);
        AddInclude(x => x.Technician);
        AddInclude(x => x.Priority);
        AddInclude(x => x.Building);
        AddInclude(x => x.BuildingLocation);
        AddInclude(x => x.Permit);
        AddInclude(x => x.Category);
        AddInclude(x => x.RequestSource);
        AddInclude(x => x.CostCode);
        AddInclude(x => x.CostCentre);
    }
}