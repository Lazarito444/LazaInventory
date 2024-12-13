using LazaInventory.Core.Application.Interfaces.Services;
using LazaInventory.Core.Domain.Entities;
using LazaInventory.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LazaInventory.Presentation.GraphQL.Queries;

public class Query
{
    private readonly AppDbContext _dbContext;

    public Query(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Item> Items()
    {
        IQueryable<Item> items = _dbContext.Set<Item>().Include(item => item.Category);
        return items;
    }
}