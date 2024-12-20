using LazaInventory.Core.Domain.Entities;

namespace LazaInventory.Core.Application.Interfaces.Repositories;

public interface IItemRepository : IGenericRepository<Item>
{
    Task<List<Item>> GetAsync();
    Task<Item?> GetAsync(int id);
    Task<List<Item>> GetLowStockItems();
    Task AddStockToItemAsync(int itemId, int quantity);
    Task RemoveStockToItemAsync(int itemId, int quantity);
}