using LazaInventory.Core.Application.Interfaces.Repositories;
using LazaInventory.Core.Domain.Common;
using LazaInventory.Infrastructure.Persistence.Contexts;

namespace LazaInventory.Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> 
        where TEntity : AppEntity
{
    private readonly AppDbContext _dbContext;

    public GenericRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(TEntity entityWithOldValues, TEntity entityWithNewValues)
    {
        _dbContext.Set<TEntity>().Entry(entityWithOldValues).CurrentValues.SetValues(entityWithNewValues);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}