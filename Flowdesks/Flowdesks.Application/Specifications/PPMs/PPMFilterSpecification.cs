using Flowdesks.Application.Features.WorkOrder.Index.Query.GetAll;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.PPMs;
using Flowdesks.Application.Requests.WorkOrder;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.PPM;
using Flowdesks.Shared.Constants;
using static Flowdesks.Shared.Constants.Common.ApplicationConstants;

namespace Flowdesks.Application.Specifications.PPMs
{
    public class PPMFilterSpecification : Specification<PPM>
    {
        private readonly ICurrentUserService _userService;

        public PPMFilterSpecification(PPMPagingRequest request, ICurrentUserService userService)
        {
            _userService = userService;

            And(x => !x.IsDeleted);

            if (request.End != null)
            {
                And(p => p.NextServiceDate <= request.End);
            }

            if (request.AssetIds != null && request.AssetIds.Count > 0)
            {
                And(p => request.AssetIds.Contains(p.AssetId));
            }

            if (request.Priorities != null && request.Priorities.Count > 0)
            {
                And(p => p.PriorityId != null && request.Priorities.Contains((Guid)p.PriorityId));
            }

            if (request.BuildingIds != null && request.BuildingIds.Count > 0)
            {
                And(p => request.BuildingIds.Contains(p.BuildingId));
            }

            if (request.Compliances != null && request.Compliances.Count > 0)
            {
                And(p => request.Compliances.Contains((Guid)p.ComplianceTypeId));
            }

            if (request.Frequencies != null && request.Frequencies.Count > 0)
            {
                And(p => request.Frequencies.Contains(p.Frequency));
            }

            if (!string.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.TaskId.Contains(request.StringSearch) ||
                p.Priority.Name.Contains(request.StringSearch) ||
                p.Asset.Name.Contains(request.StringSearch) ||
                p.Building.Name.Contains(request.StringSearch) ||
                p.Location.Name.Contains(request.StringSearch) ||
                p.Supplier.Name.Contains(request.StringSearch) ||
                p.CreatedOn.ToString().Contains(request.StringSearch) ||
                p.Technician.Name.Contains(request.StringSearch));
            }

            if (request.IsFromHistory)
            {
                And(p => p.PPMStatusTracker.Count != 0 && p.PPMStatusTracker.Any(x => x.Status.Equals(OrderStatus.ApprovedForPayment) && x.IsHistorical));
            }

            And(p => _userService.UserRoles.Any(x => x == RoleConstants.AdministratorRole) || p.Building.Users.Any(x => x.Id.ToString() == userService.UserId));
        }

        public PPMFilterSpecification(CustomFilterRequest request)
        {
            And(x => !x.IsDeleted);

            if (request.BuildingIds != null && request.BuildingIds.Count > 0)
            {
                And(p => p.BuildingId != Guid.Empty && request.BuildingIds.Contains(p.BuildingId));
            }

            if (request.SiteIds != null && request.SiteIds.Count > 0)
            {
                And(p => p.BuildingId != Guid.Empty && p.Building.SiteId != null && request.SiteIds.Contains((Guid)p.Building.SiteId));
            }

            if (request.From != null)
            {
                And(p => p.NextServiceDate.Date >= request.From.Value.Date);
            }

            if (request.To != null)
            {
                And(p => p.NextServiceDate.Date <= request.To.Value.Date);
            }

            if(request.IsForApprovedForPayment)
            {
                And(p => p.PPMStatusTracker.Count != 0 && p.PPMStatusTracker.Any(x => x.Status.Equals(OrderStatus.ApprovedForPayment)));
            }

            if (request.IsFromHistory)
            {
                And(p => p.PPMStatusTracker.Count != 0 && p.PPMStatusTracker.Any(x => x.Status.Equals(OrderStatus.ApprovedForPayment) && x.IsHistorical));
            }
        }

        public PPMFilterSpecification(CombinedPPMWorkOrderRequest request, ICurrentUserService userService)
        {
            And(x => !x.IsDeleted);

            if (request.AssetId.HasValue)
            {
                And(p => p.AssetId == request.AssetId.Value);
            }

            And(p => p.PPMStatusTracker.Any(t => t.IsHistorical && t.Status == OrderStatus.ApprovedForPayment));

            And(p => userService.UserRoles.Any(x => x == RoleConstants.AdministratorRole) ||
                     p.Building.Users.Any(x => x.Id.ToString() == userService.UserId));

            AddInclude(x => x.Asset);
            AddInclude(x => x.Building);
            AddInclude(p => p.Priority);
            AddInclude(y => y.Location);
            AddInclude(s => s.Supplier);
            AddInclude(x => x.Instruction);
            AddInclude(t => t.Technician);
            AddInclude(x => x.Skills);
            AddInclude(x => x.SuspendedPPMs);
            AddInclude(x => x.PPMStatusTracker.Where(t => t.IsHistorical && t.Status == OrderStatus.ApprovedForPayment));
            AddInclude(x => x.CostCode);
            AddInclude(x => x.CostCentre);
            AddInclude(x => x.Compliance);
        }
    }
}
