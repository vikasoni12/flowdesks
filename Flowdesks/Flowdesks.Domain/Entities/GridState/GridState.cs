using Flowdesks.Domain.Common;

namespace Flowdesks.Domain.Entities.GridState;

public class GridState : AuditableEntity<Guid>
{
    public string TableName { get; set; }//TableName Enum
    public string Data { get; set; }
    public int PageSize { get; set; }

}
