using LazaInventory.Core.Application.Interfaces.Repositories;
using LazaInventory.Core.Domain.Entities;
using LazaInventory.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LazaInventory.Infrastructure.Persistence.Repositories;

public class ItemRepository : GenericRepository<Item>, IItemRepository
{
    private readonly AppDbContext _dbContext;
    
    public ItemRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Item>> GetAsync()
    {
        List<Item> items = await _dbContext.Set<Item>().ToListAsync();
        return items;
    }

    public async Task<Item?> GetAsync(int id)
    {
        Item? item = await _dbContext.Set<Item>().FindAsync(id);
        return item;
    }

    public async Task<List<Item>> GetLowStockItems()
    {
        List<Item> items = await GetAsync();
        List<Item> lowStockItems = items.Where(item => item.Stock <= item.MinimumStock).ToList();

        return lowStockItems;
    }
}