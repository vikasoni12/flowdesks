using Flowdesks.Application.Requests.GridStates;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.GridState;
using Flowdesks.Shared.Enums;

namespace Flowdesks.Application.Specifications.GridStates
{
    public class GridStateSpecificationFilter :Specification<GridState>
    {
        public GridStateSpecificationFilter(GridStateGetAllRequest request)
        {
            if (!string.IsNullOrEmpty(Enum.GetName(typeof(TableName), request.TableName)))
            {
                var value = Enum.GetName(typeof(TableName), request.TableName);
                And(p => p.TableName.Equals(value));
            }
        }
    }
}
