using Flowdesks.Domain.Common;

namespace Flowdesks.Application.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;    
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
}
