using LazaInventory.Core.Domain.Common;

namespace LazaInventory.Core.Application.Interfaces.Repositories;

public interface IGenericRepository<TEntity> where TEntity : AppEntity
{
    Task<TEntity> CreateAsync(TEntity entity);
    Task UpdateAsync(int id, TEntity entity);
    Task DeleteAsync(TEntity entity);
}