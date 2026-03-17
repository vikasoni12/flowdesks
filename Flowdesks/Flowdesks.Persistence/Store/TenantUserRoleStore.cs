using Flowdesks.Domain.Entities.Identity;
using Flowdesks.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Persistence.Store;

public class TenantUserRoleStore : UserStore<ApplicationUser, Role, ApplicationDbContext, Guid, UserPermission, UserRole, IdentityUserLogin<Guid>, IdentityUserToken<Guid>, RolePermission>
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContext;

    public TenantUserRoleStore(ApplicationDbContext context, IHttpContextAccessor httpContext, IdentityErrorDescriber describer = null) : base(context, describer)
    {
        _context = context;
        _httpContext = httpContext;
    }

    public override async Task AddToRoleAsync(ApplicationUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
    {
        var role = _context.Roles.FirstOrDefault(x => x.TenantId == GetTenantId() && x.NormalizedName == normalizedRoleName);

        var userRole = new UserRole { Role = role, User = user };

        await _context.UserRoles.AddAsync(userRole, cancellationToken);

        //return base.AddToRoleAsync(user, normalizedRoleName, cancellationToken);
    }
    public override Task<bool> IsInRoleAsync(ApplicationUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
    {
        return _context.UserRoles.AnyAsync(x => x.Role.Name == normalizedRoleName && x.UserId == user.Id);
    }

    private Guid? GetTenantId()
    {
        if (_httpContext.HttpContext?.Items?.TryGetValue("TenantId", out object tenantIdValue) ?? false)
        {
            if (Guid.TryParse(tenantIdValue?.ToString(), out Guid guidValue))
            {
                return guidValue;
            }
        }

        return null;
    }
}
