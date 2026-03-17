using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Domain.Common;
using Flowdesks.Domain.Entities.BulkUpload;
using Flowdesks.Domain.Entities.Chat;
using Flowdesks.Domain.Entities.Documents;
using Flowdesks.Domain.Entities.GridState;
using Flowdesks.Domain.Entities.Identity;
using Flowdesks.Domain.Entities.Invoices;
using Flowdesks.Domain.Entities.Note;
using Flowdesks.Domain.Entities.Notification;
using Flowdesks.Domain.Entities.Permit;
using Flowdesks.Domain.Entities.Sites;
using Flowdesks.Domain.Entities.SystemPreferences;
using Flowdesks.Domain.Entities.Tenant;
using Flowdesks.Domain.MasterEntities;
using Flowdesks.Persistence.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Persistence.Contexts
{
    public class ApplicationDbContext : AuditableContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService, IDateTimeService dateTimeService, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Tables

        public DbSet<UserLoginDeviceHistory> UserLoginDeviceHistories { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<EntityRequiredField> EntityRequiredFields { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<DirectMessage> DirectMessages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMessage> GroupMessages { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<GridState> GridStates { get; set; }
        public DbSet<ViewState> ViewStates { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Permit> Permits { get; set; }
        public DbSet<ImportFileDetail> ImportFileDetails { get; set; }
        public DbSet<ImportFileErrorLog> ImportFileErrorLogs { get; set; }
        public DbSet<Tenant> Tenants { get; set; }

        #endregion

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            var tenantId = GetTenantId();

            if (tenantId.HasValue)
            {
                foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
                {
                    var tenantIdProperty = entry.Entity.GetType().GetProperty("TenantId");
                    if (tenantIdProperty != null && tenantIdProperty.CanWrite)
                        tenantIdProperty.SetValue(entry.Entity, tenantId.Value);
                }
            }

            var changedEntities = ChangeTracker.Entries<IAuditableEntity>().ToList();

            foreach (var entry in changedEntities)
            {
                foreach (var property in entry.Properties)
                {
                    if (property.Metadata.ClrType == typeof(DateTime) || property.Metadata.ClrType == typeof(DateTime?))
                    {
                        if (property.CurrentValue is DateTime dateTimeValue)
                        {
                            var offset = GetTimezoneOffset();
                            property.CurrentValue = SafeConvertToUtc(dateTimeValue, offset);
                        }
                    }
                }

                if (entry.Entity is IFullAuditableEntity fullAuditableEntity && fullAuditableEntity.IsDeleted && entry.State != EntityState.Deleted)
                {
                    fullAuditableEntity.DeletedOn = _dateTimeService.NowUtc;
                    fullAuditableEntity.DeletedBy = _currentUserService.UserId;

                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = _dateTimeService.NowUtc;
                        entry.Entity.CreatedBy = _currentUserService.UserId ?? string.Empty;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = _dateTimeService.NowUtc;
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        break;
                }
            }

            if (_currentUserService.UserId == null)
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            else
            {
                return await base.SaveAuditChangesAsync(_currentUserService.UserId, cancellationToken);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureConnexus();
        }

        private TimeSpan GetTimezoneOffset()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.Items.ContainsKey("TimezoneOffset"))
            {
                var offsetValue = httpContext.Items["TimezoneOffset"]?.ToString();
                if (double.TryParse(offsetValue, out var offset))
                    return TimeSpan.FromHours(offset);
            }
            return TimeSpan.Zero;
        }

        private static DateTime SafeConvertToUtc(DateTime dateTime, TimeSpan offset)
        {
            DateTime result;
            try
            {
                if (dateTime.Kind == DateTimeKind.Utc)
                {
                    return dateTime;
                }

                result = DateTime.SpecifyKind(dateTime - offset, DateTimeKind.Utc);
            }
            catch (ArgumentOutOfRangeException)
            {
                result = dateTime.Kind == DateTimeKind.Utc ? dateTime : dateTime.ToUniversalTime();
            }
            return result;
        }

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
}
