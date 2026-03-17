using Flowdesks.Application.Attributes;
using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Requests;
using Flowdesks.Application.Requests.Asset;
using Flowdesks.Application.Requests.Buildings;
using Flowdesks.Application.Requests.Contacts;
using Flowdesks.Application.Requests.Contract;
using Flowdesks.Application.Requests.Documents;
using Flowdesks.Application.Requests.PPMs;
using Flowdesks.Application.Requests.Stocks;
using Flowdesks.Application.Requests.Supplier;
using Flowdesks.Application.Requests.Technicians;
using Flowdesks.Application.Requests.WorkOrder;
using Flowdesks.Application.Responses.RequiredFields;
using Flowdesks.Domain.Entities.Identity;
using Flowdesks.Domain.Entities.Permit;
using Flowdesks.Domain.Entities.PPMs;
using Flowdesks.Domain.Entities.PurchaseOrders;
using Flowdesks.Domain.Entities.SystemPreferences;
using Flowdesks.Domain.Entities.SystemPreferences.Finance;
using Flowdesks.Domain.Entities.SystemPreferences.Stock;
using Flowdesks.Domain.Entities.SystemPreferences.Technician;
using Flowdesks.Domain.Entities.SystemPreferences.WorkOrder;
using Flowdesks.Domain.Entities.Tenant;
using Flowdesks.Domain.MasterEntities;
using Flowdesks.Persistence.Contexts;
using Flowdesks.Shared.Constants.User;
using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Permission;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Xero.NetStandard.OAuth2.Models;
using static Flowdesks.Shared.Constants.Common.ApplicationConstants;
using static Flowdesks.Shared.Permission.Permissions;
using Priority = Flowdesks.Domain.Entities.SystemPreferences.WorkOrder.Priority;

namespace Flowdesks.Persistence.Seeder;

public class DatabaseSeeder : IDatabaseSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IStringLocalizer<DatabaseSeeder> _localizer;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<DatabaseSeeder> _logger;
    private Guid? _tenantId;
    public DatabaseSeeder(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager, ILogger<DatabaseSeeder> logger, IStringLocalizer<DatabaseSeeder> localizer, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _localizer = localizer;
        _httpContextAccessor = httpContextAccessor;
    }

    public void Initialize()
    {
        _tenantId = GetTenantId();
        Task.Run(SeedPermissions).GetAwaiter().GetResult();
        Task.Run(SeedDefaultRole).GetAwaiter().GetResult();
        //Task.Run(SeedTenantData).GetAwaiter().GetResult();
        //Task.Run(SeedDefaultAdmin).GetAwaiter().GetResult(); 
        Task.Run(Country).GetAwaiter().GetResult();
        Task.Run(SeedFrequencyColors).GetAwaiter().GetResult();
        Task.Run(SeedSLASettingsColor).GetAwaiter().GetResult();
        Task.Run(SeedPriorityColor).GetAwaiter().GetResult();
        Task.Run(SeedRequiredFields).GetAwaiter().GetResult();
        Task.Run(SeedEntitiesData).GetAwaiter().GetResult();
        Task.Run(SeedWorkOrderCategories).GetAwaiter().GetResult();
        Task.Run(SeedStockCategories).GetAwaiter().GetResult();
        Task.Run(SeedPPMCompliance).GetAwaiter().GetResult();
        Task.Run(SeedPermit).GetAwaiter().GetResult();
    }

    private async Task SeedRequiredFields()
    {
        if (_tenantId == null) return;

        var entityTypes = new Dictionary<EntityType, Type>
        {
            { EntityType.Asset, typeof(AssetRequiredFields) },
            { EntityType.Supplier, typeof(CreateSupplierRequest) },
            { EntityType.Technician, typeof(CreateTechnicianRequest) },
            { EntityType.Stock, typeof(CreateStockRequest) },
            { EntityType.PurchaseOrder, typeof(AddPurchaseOrderRequest) },
            { EntityType.WorkOrder, typeof(WorkOrderRequiredFields) },
            { EntityType.PPM, typeof(PPMRequiredFields) },
            { EntityType.Building, typeof(BuildingRequiredFiled) },
            { EntityType.Document, typeof(AddDocumentRequest) },
            { EntityType.Contracts, typeof(CreateContractRequest) },
            { EntityType.Contacts, typeof(AddContactRequest) }
        };

        var requiredEntities = new List<EntityRequiredField>();
        var existingRequiredFields = await _context.EntityRequiredFields.ToListAsync();

        foreach (var entityType in entityTypes)
        {
            if (existingRequiredFields.Any(x => x.EntityType.Equals(entityType.Key.ToString()) && x.TenantId == _tenantId))
                break;

            Type entity = entityType.Value;
            var entityProperties = entity.GetProperties();

            var fieldResponses = entityProperties
            .Select(property => new FieldResponse
            {
                Title = GetDescription(property),
                Value = char.ToLower(property.Name[0]) + property.Name[1..],
                IsRequired = IsRequired(property)
            }).Where(x => x.IsRequired).ToList();

            var entities = fieldResponses.Select(x => new EntityRequiredField(_tenantId.Value, x.Value, x.Title, entityType.Key.ToString()));

            requiredEntities.AddRange(entities);
        }

        if (requiredEntities.Count > 0)
        {
            await _context.EntityRequiredFields.AddRangeAsync(requiredEntities);

            await _context.SaveChangesAsync();
        }
    }

    static string GetDescription(PropertyInfo property)
    {
        var descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(property, typeof(DescriptionAttribute));
        return descriptionAttribute?.Description ?? property.Name;
    }

    static bool IsRequired(PropertyInfo property)
    {
        IsRequiredField attribute = (IsRequiredField)Attribute.GetCustomAttribute(property, typeof(IsRequiredField));

        return attribute?.IsDefault ?? false;
    }

    private async Task SeedFrequencyColors()
    {
        if (_tenantId != null && !await _context.PPMFrequencyColors.AnyAsync(x => x.TenantId == _tenantId))
        {
            var frequencyList = new List<PPMFrequencyColor>()
            {
                new() { TenantId = _tenantId.Value, Title = FrequencyColor.Daily.ToString(), Color = "rgba(66, 35, 149, 1)", Order = 1 },
                new() { TenantId = _tenantId.Value, Title = FrequencyColor.Weekly.ToString(), Color = "rgba(255, 107, 3, 1)", Order = 2 },
                new() { TenantId = _tenantId.Value, Title = FrequencyColor.Monthly.ToString(), Color = "rgba(34, 144, 252, 1)", Order = 3 },
                new() { TenantId = _tenantId.Value, Title = FrequencyColor.Yearly.ToString(), Color = "rgba(59, 201, 74, 1)", Order = 4 },
                new() { TenantId = _tenantId.Value, Title = FrequencyColor.Suspended.ToString(), Color = "rgba(0, 0, 0, 1)", Order = 5 }
            };

            await _context.PPMFrequencyColors.AddRangeAsync(frequencyList);
            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedSLASettingsColor()
    {
        if (_tenantId != null && !await _context.SLASettings.AnyAsync(x => x.TenantId == _tenantId))
        {
            var slaSettingsList = new List<SLASettings>()
            {
                new SLASettings { TenantId = _tenantId.Value, Minutes = 45000, Color = "rgb(255, 165, 0)" },
                new SLASettings { TenantId = _tenantId.Value, Minutes = 50000, Color = "rgb(112, 195, 86)" },
                new SLASettings { TenantId = _tenantId.Value, Minutes = 60000, Color = "rgb(255, 107, 3)" },
                new SLASettings { TenantId = _tenantId.Value, Minutes = 90000, Color = "rgb(0, 91, 255)" }
            };

            await _context.SLASettings.AddRangeAsync(slaSettingsList);
            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedPriorityColor()
    {
        if (_tenantId != null && !await _context.Priority.AnyAsync(x => x.TenantId == _tenantId))
        {
            var priorityList = new List<Priority>()
            {
                new() { TenantId = _tenantId.Value, Name = "Low", Color = "rgb(116 204 121)", RankOrder = 0, Minutes = 1440 }, // 1 day
                new() { TenantId = _tenantId.Value, Name = "Medium", Color = "rgb(245, 215, 66)", RankOrder = 1, Minutes = 720 }, // 12 hours
                new() { TenantId = _tenantId.Value, Name = "High", Color = "rgb(245 111 66)", RankOrder = 2, Minutes = 240 }, // 4 hours
                new() { TenantId = _tenantId.Value, Name = "Emergency", Color = "rgb(253 53 53)", RankOrder = 3, Minutes = 60 }, // 1 hour
            };

            await _context.Priority.AddRangeAsync(priorityList);
            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedDefaultRole()
    {
        if (_tenantId != null && !await _context.Roles.AnyAsync(x => x.Name.Equals(RoleConstants.AdministratorRole, StringComparison.OrdinalIgnoreCase) && x.TenantId == _tenantId))
        {
            var roleList = new List<Role>()
            {
                new (){ TenantId = _tenantId.Value, Name=RoleConstants.AdministratorRole, NormalizedName = RoleConstants.AdministratorRole.ToUpper(),Description=RoleConstants.AdminDescription},
                new (){ TenantId = _tenantId.Value, Name=RoleConstants.Supplier, NormalizedName = RoleConstants.Supplier.ToUpper(),Description=RoleConstants.SupplierDescription},
                new (){ TenantId = _tenantId.Value, Name=RoleConstants.Technician, NormalizedName = RoleConstants.Technician.ToUpper(), Description=RoleConstants.TechnicianDescription},
                new (){ TenantId = _tenantId.Value, Name=RoleConstants.WorkRequester, NormalizedName = RoleConstants.WorkRequester.ToUpper(), Description=RoleConstants.WorkRequesterDescription},
            };
            await _context.Roles.AddRangeAsync(roleList);
            await _context.SaveChangesAsync();

            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Name == RoleConstants.AdministratorRole && x.TenantId == _tenantId);
            if (role != null) await AssignAllPermissionsToAdmin(role.Id);

            var workRequesterRole = await _roleManager.FindByNameAsync(RoleConstants.WorkRequester);
            await _context.RolePermissions.AddAsync(new RolePermission
            {
                ClaimType = PermissionConstants.Name,
                ClaimValue = WorkRequestPermissions.Create,
                RoleId = workRequesterRole.Id
            });

            var technicianRole = await _roleManager.FindByNameAsync(RoleConstants.Technician);
            var supplierRole = await _roleManager.FindByNameAsync(RoleConstants.Supplier);
            var permissionsToAdd = new List<RolePermission>
            {
                new RolePermission { ClaimType = PermissionConstants.Name, ClaimValue = SupportPermissions.View, RoleId = technicianRole.Id },
                new RolePermission { ClaimType = PermissionConstants.Name, ClaimValue = SupportResponsePermissions.All, RoleId = technicianRole.Id },
                new RolePermission { ClaimType = PermissionConstants.Name, ClaimValue = WorkOrderPermissions.View, RoleId = technicianRole.Id },
                new RolePermission { ClaimType = PermissionConstants.Name, ClaimValue = PPMPermissions.View, RoleId = technicianRole.Id },
                new RolePermission { ClaimType = PermissionConstants.Name, ClaimValue = ProcedurePermissions.View, RoleId = technicianRole.Id },
            };

            permissionsToAdd.AddRange(new List<RolePermission>
            {
                new RolePermission { ClaimType = PermissionConstants.Name, ClaimValue = SupportPermissions.View, RoleId = supplierRole.Id },
                new RolePermission { ClaimType = PermissionConstants.Name, ClaimValue = SupportResponsePermissions.All, RoleId = supplierRole.Id },
                new RolePermission { ClaimType = PermissionConstants.Name, ClaimValue = WorkOrderPermissions.View, RoleId = supplierRole.Id },
                new RolePermission { ClaimType = PermissionConstants.Name, ClaimValue = PPMPermissions.View, RoleId = supplierRole.Id },
                new RolePermission { ClaimType = PermissionConstants.Name, ClaimValue = ProcedurePermissions.View, RoleId = supplierRole.Id },
            });

            await _context.RolePermissions.AddRangeAsync(permissionsToAdd);
            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedDefaultAdmin()
    {
        var adminExists = await _userManager.FindByEmailAsync("admin@admin.com");
        var adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);

        if (adminExists == null && _tenantId != null)
        {
            //Check if Role Exists

            if (adminRoleInDb == null)
            {
                var adminRole = new Role(RoleConstants.AdministratorRole, _localizer["Administrator role with full permissions"]);
                await _roleManager.CreateAsync(adminRole);
                _logger.LogInformation(_localizer["Seeded Administrator Role."]);
            }

            var admin = new ApplicationUser
            {
                TenantId = _tenantId.Value,
                FirstName = "admin",
                LastName = "admin",
                Email = "admin@admin.com",
                UserName = "admin@admin.com",
                EmailConfirmed = true,
                RefreshToken = string.Empty,
                ProfilePictureDataUrl = string.Empty,
                IsActive = true
            };

            await _userManager.CreateAsync(admin, UserConstants.DefaultPassword);

            var result = await _userManager.AddToRoleAsync(admin, RoleConstants.AdministratorRole);

            if (result.Succeeded)
            {
                _logger.LogInformation(_localizer["Seeded Default SuperAdmin User."]);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError(error.Description);
                }
            }
        }

        var roles = await _userManager.GetRolesAsync(adminExists);
        if (roles.Count == 0)
            await _userManager.AddToRoleAsync(adminExists, RoleConstants.AdministratorRole);

        //assign all permissions 
        adminRoleInDb ??= await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
        await AssignAllPermissionsToAdmin(adminRoleInDb.Id);

        var workRequesterRole = await _roleManager.FindByNameAsync(RoleConstants.WorkRequester);
        await _context.RolePermissions.AddAsync(new RolePermission
        {
            ClaimType = PermissionConstants.Name,
            ClaimValue = WorkRequestPermissions.Create,
            RoleId = workRequesterRole.Id
        });

        var technicianRole = await _roleManager.FindByNameAsync(RoleConstants.Technician);
        var supplierRole = await _roleManager.FindByNameAsync(RoleConstants.Supplier);
        var permissionsToAdd = new List<RolePermission>
        {
            new RolePermission { ClaimType = PermissionConstants.Name, ClaimValue = SupportPermissions.View, RoleId = technicianRole.Id },
            new RolePermission { ClaimType = PermissionConstants.Name, ClaimValue = SupportResponsePermissions.All, RoleId = technicianRole.Id },
            new RolePermission { ClaimType = PermissionConstants.Name, ClaimValue = WorkOrderPermissions.View, RoleId = technicianRole.Id },
            new RolePermission { ClaimType = PermissionConstants.Name, ClaimValue = PPMPermissions.View, RoleId = technicianRole.Id },
            new RolePermission { ClaimType = PermissionConstants.Name, ClaimValue = ProcedurePermissions.View, RoleId = technicianRole.Id },
        };

        permissionsToAdd.AddRange(new List<RolePermission>
        {
            new RolePermission { ClaimType = PermissionConstants.Name, ClaimValue = SupportPermissions.View, RoleId = supplierRole.Id },
            new RolePermission { ClaimType = PermissionConstants.Name, ClaimValue = SupportResponsePermissions.All, RoleId = supplierRole.Id },
            new RolePermission { ClaimType = PermissionConstants.Name, ClaimValue = WorkOrderPermissions.View, RoleId = supplierRole.Id },
            new RolePermission { ClaimType = PermissionConstants.Name, ClaimValue = PPMPermissions.View, RoleId = supplierRole.Id },
            new RolePermission { ClaimType = PermissionConstants.Name, ClaimValue = ProcedurePermissions.View, RoleId = supplierRole.Id },
        });

        await _context.RolePermissions.AddRangeAsync(permissionsToAdd);
        await _context.SaveChangesAsync();
    }

    private async Task SeedTenantData()
    {
        if (!await _context.Tenants.AnyAsync())
        {
            var tenant = new Domain.Entities.Tenant.Tenant { Name = "Step2gen" };
            await _context.Tenants.AddAsync(tenant);

            var admin = new ApplicationUser
            {
                TenantId = tenant.Id,
                FirstName = "Step2gen",
                Email = "step2gen@gmail.com",
                UserName = "step2gen@gmail.com",
                EmailConfirmed = true,
                RefreshToken = string.Empty,
                ProfilePictureDataUrl = string.Empty,
                IsActive = true
            };

            await _userManager.CreateAsync(admin, UserConstants.DefaultPassword);
            await _userManager.AddToRoleAsync(admin, RoleConstants.AdministratorRole);

            await _context.SaveChangesAsync();
            _tenantId = tenant.Id;
        }
        else
        {
            _tenantId = GetTenantId();
        }
    }

    public async Task SeedPermissions()
    {
        var permissions = new List<Permission>();
        var existingPermissions = await _context.Permissions.ToListAsync();

        foreach (var nestedClass in typeof(Permissions).GetNestedTypes())
        {
            var groupName = nestedClass.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;

            foreach (var field in nestedClass.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var permissionName = (string)field.GetValue(null);
                var permissionDisplayName = field.GetCustomAttribute<DisplayAttribute>()?.Name ?? permissionName;

                // Check if the permission already exists
                if (_tenantId != null && !existingPermissions.Any(p => p.Group.Equals(groupName, StringComparison.OrdinalIgnoreCase) && p.Description.Equals(permissionName, StringComparison.OrdinalIgnoreCase) && p.TenantId == _tenantId))
                {
                    permissions.Add(new Permission(_tenantId.Value, permissionName, groupName, permissionDisplayName));
                }
            }
        }

        if (permissions?.Count > 0)
        {
           await _context.Permissions.AddRangeAsync(permissions);
           await _context.SaveChangesAsync();
        }
    }

    public async Task Country()
    {
        //Task.Run(async () =>
        //{
            var countryList = new List<Country>
            {
                  new Country  { Name= "Afghanistan", Code="AF" },
                  new Country {Name= "land Islands", Code= "AX" },
                  new Country {Name= "Albania", Code= "AL" },
                  new Country {Name= "Algeria", Code= "DZ" },
                  new Country {Name= "American Samoa", Code= "AS" },
                  new Country {Name= "AndorrA", Code= "AD" },
                  new Country {Name= "Angola", Code= "AO" },
                  new Country {Name= "Anguilla", Code= "AI" },
                  new Country {Name= "Antarctica", Code= "AQ" },
                  new Country {Name= "Antigua and Barbuda", Code= "AG" },
                  new Country {Name= "Argentina", Code= "AR" },
                  new Country {Name= "Armenia", Code= "AM" },
                  new Country {Name= "Aruba", Code= "AW" },
                  new Country {Name= "Australia", Code= "AU" },
                  new Country {Name= "Austria", Code= "AT" },
                  new Country {Name= "Azerbaijan", Code= "AZ" },
                  new Country {Name= "Bahamas", Code= "BS" },
                  new Country {Name= "Bahrain", Code= "BH" },
                  new Country {Name= "Bangladesh", Code= "BD" },
                  new Country {Name= "Barbados", Code= "BB" },
                  new Country {Name= "Belarus", Code= "BY" },
                  new Country {Name= "Belgium", Code= "BE" },
                  new Country {Name= "Belize", Code= "BZ" },
                  new Country {Name= "Benin", Code= "BJ" },
                  new Country {Name= "Bermuda", Code= "BM" },
                  new Country {Name= "Bhutan", Code= "BT" },
                  new Country {Name= "Bolivia", Code= "BO" },
                  new Country {Name= "Bosnia and Herzegovina", Code= "BA" },
                  new Country {Name= "Botswana", Code= "BW" },
                  new Country {Name= "Bouvet Island", Code= "BV" },
                  new Country {Name= "Brazil", Code= "BR" },
                  new Country {Name= "British Indian Ocean Territory", Code= "IO" },
                  new Country {Name= "Brunei Darussalam", Code= "BN" },
                  new Country {Name= "Bulgaria", Code= "BG" },
                  new Country {Name= "Burkina Faso", Code= "BF" },
                  new Country {Name= "Burundi", Code= "BI" },
                  new Country {Name= "Cambodia", Code= "KH" },
                  new Country {Name= "Cameroon", Code= "CM" },
                  new Country {Name= "Canada", Code= "CA" },
                  new Country {Name= "Cape Verde", Code= "CV" },
                  new Country {Name= "Cayman Islands", Code= "KY" },
                  new Country {Name= "Central African Republic", Code= "CF" },
                  new Country {Name= "Chad", Code= "TD" },
                  new Country {Name= "Chile", Code= "CL" },
                  new Country {Name= "China", Code= "CN" },
                  new Country {Name= "Christmas Island", Code= "CX" },
                  new Country {Name= "Cocos (Keeling) Islands", Code= "CC" },
                  new Country {Name= "Colombia", Code= "CO" },
                  new Country {Name= "Comoros", Code= "KM" },
                  new Country {Name= "Congo", Code= "CG" },
                  new Country {Name= "Congo, The Democratic Republic of the", Code= "CD" },
                  new Country {Name= "Cook Islands", Code= "CK" },
                  new Country {Name= "Costa Rica", Code= "CR" },
                  new Country {Name= "Cote DIvoire", Code= "CI" },
                  new Country {Name= "Croatia", Code= "HR" },
                  new Country {Name= "Cuba", Code= "CU" },
                  new Country {Name= "Cyprus", Code= "CY" },
                  new Country {Name= "Czech Republic", Code= "CZ" },
                  new Country {Name= "Denmark", Code= "DK" },
                  new Country {Name= "Djibouti", Code= "DJ" },
                  new Country {Name= "Dominica", Code= "DM" },
                  new Country {Name= "Dominican Republic", Code= "DO" },
                  new Country {Name= "Ecuador", Code= "EC" },
                  new Country {Name= "Egypt", Code= "EG" },
                  new Country {Name= "El Salvador", Code= "SV" },
                  new Country {Name= "Equatorial Guinea", Code= "GQ" },
                  new Country {Name= "Eritrea", Code= "ER" },
                  new Country {Name= "Estonia", Code= "EE" },
                  new Country {Name= "Ethiopia", Code= "ET" },
                  new Country {Name= "Falkland Islands (Malvinas)", Code= "FK" },
                  new Country {Name= "Faroe Islands", Code= "FO" },
                  new Country {Name= "Fiji", Code= "FJ" },
                  new Country {Name= "Finland", Code= "FI" },
                  new Country {Name= "France", Code= "FR" },
                  new Country {Name= "French Guiana", Code= "GF" },
                  new Country {Name= "French Polynesia", Code= "PF" },
                  new Country {Name= "French Southern Territories", Code= "TF" },
                  new Country {Name= "Gabon", Code= "GA" },
                  new Country {Name= "Gambia", Code= "GM" },
                  new Country {Name= "Georgia", Code= "GE" },
                  new Country {Name= "Germany", Code= "DE" },
                  new Country {Name= "Ghana", Code= "GH" },
                  new Country {Name= "Gibraltar", Code= "GI" },
                  new Country {Name= "Greece", Code= "GR" },
                  new Country {Name= "Greenland", Code= "GL" },
                  new Country {Name= "Grenada", Code= "GD" },
                  new Country {Name= "Guadeloupe", Code= "GP" },
                  new Country {Name= "Guam", Code= "GU" },
                  new Country {Name= "Guatemala", Code= "GT" },
                  new Country {Name= "Guernsey", Code= "GG" },
                  new Country {Name= "Guinea", Code= "GN" },
                  new Country {Name= "Guinea-Bissau", Code= "GW" },
                  new Country {Name= "Guyana", Code= "GY" },
                  new Country {Name= "Haiti", Code= "HT" },
                  new Country {Name= "Heard Island and Mcdonald Islands", Code= "HM" },
                  new Country {Name= "Holy See (Vatican City State)", Code= "VA" },
                  new Country {Name= "Honduras", Code= "HN" },
                  new Country {Name= "Hong Kong", Code= "HK" },
                  new Country {Name= "Hungary", Code= "HU" },
                  new Country {Name= "Iceland", Code= "IS" },
                  new Country {Name= "India", Code= "IN" },
                  new Country {Name= "Indonesia", Code= "ID" },
                  new Country {Name= "Iran, Islamic Republic Of", Code= "IR" },
                  new Country {Name= "Iraq", Code= "IQ" },
                  new Country {Name= "Ireland", Code= "IE" },
                  new Country {Name= "Isle of Man", Code= "IM" },
                  new Country {Name= "Israel", Code= "IL" },
                  new Country {Name= "Italy", Code= "IT" },
                  new Country {Name= "Jamaica", Code= "JM" },
                  new Country {Name= "Japan", Code= "JP" },
                  new Country {Name= "Jersey", Code= "JE" },
                  new Country {Name= "Jordan", Code= "JO" },
                  new Country {Name= "Kazakhstan", Code= "KZ" },
                  new Country {Name= "Kenya", Code= "KE" },
                  new Country {Name= "Kiribati", Code= "KI" },
                  new Country {Name= "Korea, Democratic PeopleS Republic of", Code= "KP" },
                  new Country {Name= "Korea, Republic of", Code= "KR" },
                  new Country {Name= "Kuwait", Code= "KW" },
                  new Country {Name= "Kyrgyzstan", Code= "KG" },
                  new Country {Name= "Lao PeopleS Democratic Republic", Code= "LA" },
                  new Country {Name= "Latvia", Code= "LV" },
                  new Country {Name= "Lebanon", Code= "LB" },
                  new Country {Name= "Lesotho", Code= "LS" },
                  new Country {Name= "Liberia", Code= "LR" },
                  new Country {Name= "Libyan Arab Jamahiriya", Code= "LY" },
                  new Country {Name= "Liechtenstein", Code= "LI" },
                  new Country {Name= "Lithuania", Code= "LT" },
                  new Country {Name= "Luxembourg", Code= "LU" },
                  new Country {Name= "Macao", Code= "MO" },
                  new Country {Name= "Macedonia, The Former Yugoslav Republic of", Code= "MK" },
                  new Country {Name= "Madagascar", Code= "MG" },
                  new Country {Name= "Malawi", Code= "MW" },
                  new Country {Name= "Malaysia", Code= "MY" },
                  new Country {Name= "Maldives", Code= "MV" },
                  new Country {Name= "Mali", Code= "ML" },
                  new Country {Name= "Malta", Code= "MT" },
                  new Country {Name= "Marshall Islands", Code= "MH" },
                  new Country {Name= "Martinique", Code= "MQ" },
                  new Country {Name= "Mauritania", Code= "MR" },
                  new Country {Name= "Mauritius", Code= "MU" },
                  new Country {Name= "Mayotte", Code= "YT" },
                  new Country {Name= "Mexico", Code= "MX" },
                  new Country {Name= "Micronesia, Federated States of", Code= "FM" },
                  new Country {Name= "Moldova, Republic of", Code= "MD" },
                  new Country {Name= "Monaco", Code= "MC" },
                  new Country {Name= "Mongolia", Code= "MN" },
                  new Country {Name= "Montenegro", Code= "ME" },
                  new Country {Name= "Montserrat", Code= "MS" },
                  new Country {Name= "Morocco", Code= "MA" },
                  new Country {Name= "Mozambique", Code= "MZ" },
                  new Country {Name= "Myanmar", Code= "MM" },
                  new Country {Name= "Namibia", Code= "NA" },
                  new Country {Name= "Nauru", Code= "NR" },
                  new Country {Name= "Nepal", Code= "NP" },
                  new Country {Name= "Netherlands", Code= "NL" },
                  new Country {Name= "Netherlands Antilles", Code= "AN" },
                  new Country {Name= "New Caledonia", Code= "NC" },
                  new Country {Name= "New Zealand", Code= "NZ" },
                  new Country {Name= "Nicaragua", Code= "NI" },
                  new Country {Name= "Niger", Code= "NE" },
                  new Country {Name= "Nigeria", Code= "NG" },
                  new Country {Name= "Niue", Code= "NU" },
                  new Country {Name= "Norfolk Island", Code= "NF" },
                  new Country {Name= "Northern Mariana Islands", Code= "MP" },
                  new Country {Name= "Norway", Code= "NO" },
                  new Country {Name= "Oman", Code= "OM" },
                  new Country {Name= "Pakistan", Code= "PK" },
                  new Country {Name= "Palau", Code= "PW" },
                  new Country {Name= "Palestinian Territory, Occupied", Code= "PS" },
                  new Country {Name= "Panama", Code= "PA" },
                  new Country {Name= "Papua New Guinea", Code= "PG" },
                  new Country {Name= "Paraguay", Code= "PY" },
                  new Country {Name= "Peru", Code= "PE" },
                  new Country {Name= "Philippines", Code= "PH" },
                  new Country {Name= "Pitcairn", Code= "PN" },
                  new Country {Name= "Poland", Code= "PL" },
                  new Country {Name= "Portugal", Code= "PT" },
                  new Country {Name= "Puerto Rico", Code= "PR" },
                  new Country {Name= "Qatar", Code= "QA" },
                  new Country {Name= "Reunion", Code= "RE" },
                  new Country {Name= "Romania", Code= "RO" },
                  new Country {Name= "Russian Federation", Code= "RU" },
                  new Country {Name= "RWANDA", Code= "RW" },
                  new Country {Name= "Saint Helena", Code= "SH" },
                  new Country {Name= "Saint Kitts and Nevis", Code= "KN" },
                  new Country {Name= "Saint Lucia", Code= "LC" },
                  new Country {Name= "Saint Pierre and Miquelon", Code= "PM" },
                  new Country {Name= "Saint Vincent and the Grenadines", Code= "VC" },
                  new Country {Name= "Samoa", Code= "WS" },
                  new Country {Name= "San Marino", Code= "SM" },
                  new Country {Name= "Sao Tome and Principe", Code= "ST" },
                  new Country {Name= "Saudi Arabia", Code= "SA" },
                  new Country {Name= "Senegal", Code= "SN" },
                  new Country {Name= "Serbia", Code= "RS" },
                  new Country {Name= "Seychelles", Code= "SC" },
                  new Country {Name= "Sierra Leone", Code= "SL" },
                  new Country {Name= "Singapore", Code= "SG" },
                  new Country {Name= "Slovakia", Code= "SK" },
                  new Country {Name= "Slovenia", Code= "SI" },
                  new Country {Name= "Solomon Islands", Code= "SB" },
                  new Country {Name= "Somalia", Code= "SO" },
                  new Country {Name= "South Africa", Code= "ZA" },
                  new Country {Name= "South Georgia and the South Sandwich Islands", Code= "GS" },
                  new Country {Name= "Spain", Code= "ES" },
                  new Country {Name= "Sri Lanka", Code= "LK" },
                  new Country {Name= "Sudan", Code= "SD" },
                  new Country {Name= "Suriname", Code= "SR" },
                  new Country {Name= "Svalbard and Jan Mayen", Code= "SJ" },
                  new Country {Name= "Swaziland", Code= "SZ" },
                  new Country {Name= "Sweden", Code= "SE" },
                  new Country {Name= "Switzerland", Code= "CH" },
                  new Country {Name= "Syrian Arab Republic", Code= "SY" },
                  new Country {Name= "Taiwan, Province of China", Code= "TW" },
                  new Country {Name= "Tajikistan", Code= "TJ" },
                  new Country {Name= "Tanzania, United Republic of", Code= "TZ" },
                  new Country {Name= "Thailand", Code= "TH" },
                  new Country {Name= "Timor-Leste", Code= "TL" },
                  new Country {Name= "Togo", Code= "TG" },
                  new Country {Name= "Tokelau", Code= "TK" },
                  new Country {Name= "Tonga", Code= "TO" },
                  new Country {Name= "Trinidad and Tobago", Code= "TT" },
                  new Country {Name= "Tunisia", Code= "TN" },
                  new Country {Name= "Turkey", Code= "TR" },
                  new Country {Name= "Turkmenistan", Code= "TM" },
                  new Country {Name= "Turks and Caicos Islands", Code= "TC" },
                  new Country {Name= "Tuvalu", Code= "TV" },
                  new Country {Name= "Uganda", Code= "UG" },
                  new Country {Name= "Ukraine", Code= "UA" },
                  new Country {Name= "United Arab Emirates", Code= "AE" },
                  new Country {Name= "United Kingdom", Code= "GB" },
                  new Country {Name= "United States", Code= "US" },
                  new Country {Name= "United States Minor Outlying Islands", Code= "UM" },
                  new Country {Name= "Uruguay", Code= "UY" },
                  new Country {Name= "Uzbekistan", Code= "UZ" },
                  new Country {Name= "Vanuatu", Code= "VU" },
                  new Country {Name= "Venezuela", Code= "VE" },
                  new Country {Name= "Viet Nam", Code= "VN" },
                  new Country {Name= "Virgin Islands, British", Code= "VG" },
                  new Country {Name= "Virgin Islands, U.S.", Code= "VI" },
                  new Country {Name= "Wallis and Futuna", Code= "WF" },
                  new Country {Name= "Western Sahara", Code= "EH" },
                  new Country {Name= "Yemen", Code= "YE" },
                  new Country {Name= "Zambia", Code= "ZM" },
                  new Country {Name= "Zimbabwe", Code= "ZW" },
            };

            if (countryList?.Count > 0)
            {
                var CountryExist = await _context.Countries.FirstOrDefaultAsync();
                if (CountryExist == null)
                {
                    await _context.Countries.AddRangeAsync(countryList);
                    await _context.SaveChangesAsync();
                }

           }
        //}).GetAwaiter().GetResult();
    }

    public async Task AssignAllPermissionsToAdmin(Guid roleId)
    {
        var permissions = Permissions.GetRegisteredPermissions();

        var alreadyAssignedPermissions = await _context.RolePermissions.Where(x => x.RoleId.Equals(roleId)).ToListAsync();

        // Remove permissions that are no longer in the permissions list
        var outdatedPermissions = alreadyAssignedPermissions
            .Where(x => !permissions.Contains(x.ClaimValue))
            .ToList();

        if (outdatedPermissions.Count > 0)
        {
            _context.RolePermissions.RemoveRange(outdatedPermissions);
        }

        var unassignedPermissions = permissions.Except(alreadyAssignedPermissions.Select(x => x.ClaimValue)).ToList();

        await _context.RolePermissions.AddRangeAsync(unassignedPermissions.Select(x => new RolePermission
        {
            ClaimType = PermissionConstants.Name,
            ClaimValue = x,
            RoleId = roleId
        }));

        await _context.SaveChangesAsync();
    }

    public async Task SeedEntitiesData()
    {
        if (_tenantId != null && !await _context.AssetCondition.AnyAsync(x => x.TenantId == _tenantId))
        {
            var conditions = new List<(string Name, int Order)>
            {
                ("Excellent", 5),
                ("Good", 4),
                ("Fair", 3),
                ("Poor", 2),
                ("Near failure or failed", 1)
            };

            await _context.AssetCondition.AddRangeAsync(conditions.Select(condition => new Domain.Entities.Asset.AssetCondition
            {
                TenantId = _tenantId.Value,
                Name = condition.Name,
                Order = condition.Order
            }));
        }

        if (_tenantId != null && !await _context.WORequestSources.AnyAsync(x => x.TenantId == _tenantId))
        {
            var sources = new List<string> { "Work Requester Portal", "Call", "Email", "In Person" };
            await _context.WORequestSources.AddRangeAsync(sources.Select(source => new WORequestSource { TenantId = _tenantId.Value, Name = source }));
        }

        if (_tenantId != null && !await _context.SupplierCategory.AnyAsync(x => x.TenantId == _tenantId))
        {
            var categories = new List<string> { "Services", "Goods", "Goods and services" };
            await _context.SupplierCategory.AddRangeAsync(categories.Select(ct => new Domain.Entities.Suppliers.SupplierCategory { TenantId = _tenantId.Value, Name = ct }));
        }

        if (_tenantId != null && !await _context.Skills.AnyAsync(x => x.TenantId == _tenantId))
        {
            var skills = new List<string> { "Plumber", "Carpentry", "Electrician", "Mechanical", "HVAC", "Cleaner", "Fire Inspector", "Pest Control", "Gardener", "Locksmith", "Security", "Painter", "Asbestos" };
            await _context.Skills.AddRangeAsync(skills.Select(ct => new Skill { TenantId = _tenantId.Value, Name = ct }));
        }

        if (_tenantId != null && !await _context.Qualifications.AnyAsync(x => x.TenantId == _tenantId))
        {
            var qualifications = new List<string> { "White Card", "Blue Card", "Working with Heights", "Confined Space", "Asbestos Removal", "Liability Insurance" };
            await _context.Qualifications.AddRangeAsync(qualifications.Select(ct => new Qualification { TenantId = _tenantId.Value, Name = ct }));
        }

        if (_tenantId != null && !await _context.POAuthorizations.AnyAsync(x => x.TenantId == _tenantId))
        {
            var authrizationLimit = new POAuthorization { TenantId = _tenantId.Value, AuthorizationLimit = 1000, IsAuthorized = true, IsSelfAuthorize = false };
            _context.POAuthorizations.Add(authrizationLimit);
        }

        if (_tenantId != null && !await _context.CostCodes.AnyAsync(x => x.TenantId == _tenantId))
        {
            var codes = new List<string> { "Reactive", "Planned maintenance" };
            await _context.CostCodes.AddRangeAsync(codes.Select(ct => new CostCode { TenantId = _tenantId.Value, Name = ct }));
        }

        await _context.SaveChangesAsync();
    }

    public async Task SeedWorkOrderCategories()
    {
        var anyCategoriesExist = await _context.WorkOrderCategories.AnyAsync(x => x.TenantId == _tenantId);

        if (_tenantId != null && !anyCategoriesExist)
        {
            var categories = new List<WorkOrderCategory>()
            {
              new() { TenantId = _tenantId.Value, Name = "Mechanical" },
              new() { TenantId = _tenantId.Value, Name = "Plumbing" },
              new() { TenantId = _tenantId.Value, Name = "Electrical" },
              new() { TenantId = _tenantId.Value, Name = "Safety" },
              new() { TenantId = _tenantId.Value, Name = "Refrigeration" },
              new() { TenantId = _tenantId.Value, Name = "HVAC" },
              new() { TenantId = _tenantId.Value, Name = "Inspection" },
            };

            await _context.WorkOrderCategories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SeedStockCategories()
    {
        var anyCategoriesExist = await _context.StockCategories.AnyAsync(x => x.TenantId == _tenantId);

        if (_tenantId != null && !anyCategoriesExist)
        {
            var categories = new List<StockCategory>()
            {
              new() { TenantId = _tenantId.Value, Name = "Electrical Components" },
              new() { TenantId = _tenantId.Value, Name = "Plumbing Components" },
              new() { TenantId = _tenantId.Value, Name = "Materials and raw goods" },
              new() { TenantId = _tenantId.Value, Name = "Safety Equipment" },
              new() { TenantId = _tenantId.Value, Name = "Hardware" },
              new() { TenantId = _tenantId.Value, Name = "Tools" },
              new() { TenantId = _tenantId.Value, Name = "Equipment Components" },
              new() { TenantId = _tenantId.Value, Name = "Consumables" }
            };

            await _context.StockCategories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SeedPPMCompliance()
    {
        var anyComplianceExist = await _context.ComplianceTypes.AnyAsync(x => x.TenantId == _tenantId);

        if (_tenantId != null && !anyComplianceExist)
        {
            var categories = new List<ComplianceType>()
            {
              new() { TenantId = _tenantId.Value, Name = "Yes" },
              new() { TenantId = _tenantId.Value, Name = "No" },
            };

            await _context.ComplianceTypes.AddRangeAsync(categories);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SeedPermit()
    {
        var anyPermitExist = await _context.Permits.AnyAsync(x => x.TenantId == _tenantId);

        if (_tenantId != null && !anyPermitExist)
        {
            var permits = new List<Permit>()
            {
              new() { TenantId = _tenantId.Value, Name = "Working at height" },
              new() { TenantId = _tenantId.Value, Name = "Heavy Machinery" },
              new() { TenantId = _tenantId.Value, Name = "Confined Space" },
              new() { TenantId = _tenantId.Value, Name = "Hot works" },
            };

            await _context.Permits.AddRangeAsync(permits);
            await _context.SaveChangesAsync();
        }
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