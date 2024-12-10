using LazaInventory.Core.Domain.Common;

namespace LazaInventory.Core.Application.Interfaces.Repositories;

public interface IGenericRepository<TEntity> where TEntity : AppEntity
{
    Task<TEntity> CreateAsync(TEntity entity);
    Task UpdateAsync(TEntity entityWithOldValues, TEntity entityWithNewValues);
    Task DeleteAsync(TEntity entity);
}