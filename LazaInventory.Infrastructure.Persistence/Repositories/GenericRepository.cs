using System.Net;
using LazaInventory.Core.Application.Exceptions;
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

    public async Task UpdateAsync(int id, TEntity entity)
    {
        TEntity? entityToUpdate = await _dbContext.Set<TEntity>().FindAsync(id);

        // if (entityToUpdate == null)
        // {
        //     throw new ApiException(HttpStatusCode.NotFound,
        //         $"Didn't find any instance of entity {typeof(TEntity)} with id {id}");
        // }
        _dbContext.Set<TEntity>().Entry(entityToUpdate!).CurrentValues.SetValues(entity);
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}