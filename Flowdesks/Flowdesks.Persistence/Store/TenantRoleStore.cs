using Flowdesks.Domain.Entities.Identity;
using Flowdesks.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Flowdesks.Persistence.Store;

public class TenantRoleStore : RoleStore<Role, ApplicationDbContext, Guid>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantRoleStore(
        IHttpContextAccessor httpContextAccessor,
        ApplicationDbContext context,
        IdentityErrorDescriber describer = null)
        : base(context, describer)
    {

        _httpContextAccessor = httpContextAccessor;
    }

    public override IQueryable<Role> Roles => GetTenantId() != null ? base.Roles.Where(r => r.TenantId == GetTenantId()) : base.Roles;

    private Guid? GetTenantId()
    {
        if (_httpContextAccessor.HttpContext?.Items?.TryGetValue("TenantId", out object tenantIdValue) ?? false)
        {
            if (Guid.TryParse(tenantIdValue?.ToString(), out Guid guidValue))
            {
                return guidValue;
            }
        }

        return null;
    }
}