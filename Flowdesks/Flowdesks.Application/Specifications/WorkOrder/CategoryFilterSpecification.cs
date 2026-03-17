using Flowdesks.Application.Requests.WorkOrder.Category;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.SystemPreferences.WorkOrder;

namespace Flowdesks.Application.Specifications.WorkOrder;

public class CategoryFilterSpecification :Specification<WorkOrderCategory>
{
    public CategoryFilterSpecification(WorkOrderCategoryPagingRequest request)
    {
        if (!string.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.Name.Contains(request.StringSearch));
        }
    }
}
