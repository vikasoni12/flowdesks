using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Domain.Entities.Identity;
using Flowdesks.Infrastructure.Repositories;
using Flowdesks.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Flowdesks.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
                .AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
        }
    }
}
