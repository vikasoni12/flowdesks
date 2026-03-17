using Microsoft.AspNetCore.Identity;

namespace Flowdesks.Domain.Entities.Identity;

public class RolePermission : IdentityRoleClaim<Guid>
{
    public virtual Role Role { get; set; }

}