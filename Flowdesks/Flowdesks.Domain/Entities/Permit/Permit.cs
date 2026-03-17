using Flowdesks.Domain.Common;

namespace Flowdesks.Domain.Entities.Permit;

public class Permit : IEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}