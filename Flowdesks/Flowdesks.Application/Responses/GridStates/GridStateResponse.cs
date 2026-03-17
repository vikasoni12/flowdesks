using Flowdesks.Application.Requests.GridStates;

namespace Flowdesks.Application.Responses.GridStates;

public class GridStateResponse
{
    public int PageSize {  get; set; }
    public List<ColumnRequest> Columns { get; set; }
}
