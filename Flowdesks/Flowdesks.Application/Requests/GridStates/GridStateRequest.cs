using Flowdesks.Domain.Entities.GridState;
using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.GridStates;

public class GridStateRequest : CreateEditRequest<GridState>, IRequest<Result<int>>
{
    public int PageSize { get; set; }
    public TableName TableName { get; set; }
    public List<ColumnRequest> Columns { get; set; }
}
