using LazaInventory.Core.Application.Dtos.Item;
using LazaInventory.Core.Domain.Entities;

namespace LazaInventory.Core.Application.Interfaces.Services;

public interface IItemService
{
    Task<Item?> GetAsync(int id);
    Task<List<Item>> GetAsync();
    Task<Item> CreateAsync(SaveItemDto saveItemDto, string imagePath);
    Task UpdateAsync(int id, UpdateItemDto saveItemDto, string imagePath);
    Task DeleteAsync(int id);

    Task<List<Item>> GetLowStockItemsAsync();
}