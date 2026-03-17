using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.CSV;
using Flowdesks.Application.Interfaces.Email;
using Flowdesks.Application.Interfaces.Email.IEmailPopulate;
using Flowdesks.Application.Interfaces.Identity;
using Flowdesks.Application.Interfaces.Notification;
using Flowdesks.Application.Interfaces.SystemPreferences;
using Flowdesks.Infrastructure.Services.Common;
using Flowdesks.Infrastructure.Services.CSV;
using Flowdesks.Infrastructure.Services.Email;
using Flowdesks.Infrastructure.Services.Email.EmailPopulate;
using Flowdesks.Infrastructure.Services.Identity;
using Flowdesks.Infrastructure.Services.Notification;
using Flowdesks.Infrastructure.Services.SystemPreferences;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Flowdesks.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRolePermissionService, RolePermissionService>();
            services.AddTransient<ITokenService, IdentityService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserService, UserService>();
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddTransient<IEmailPopulateBody, EmailPopulateBody>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<ICsvExportService, CsvExportService>();

            services.AddScoped<IRequiredFieldService, RequiredFieldService>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IBulkUploadService, BulkUploadService>();          
            services.AddScoped<IDuplicateRecordsForBulkService, DuplicateRecordsForBulkService>();          


            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
