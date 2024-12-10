using System.Net;
using AutoMapper;
using LazaInventory.Core.Application.Dtos.Item;
using LazaInventory.Core.Application.Exceptions;
using LazaInventory.Core.Application.Interfaces.Repositories;
using LazaInventory.Core.Application.Interfaces.Services;
using LazaInventory.Core.Domain.Entities;

namespace LazaInventory.Core.Application.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;
    private readonly IMapper _mapper;

    public ItemService(IItemRepository itemRepository, IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper = mapper;
    }

    public async Task<Item?> GetAsync(int id)
    {
        Item? item = await _itemRepository.GetAsync(id);
        return item;
    }
    
    public async Task<List<Item>> GetAsync()
    {
        List<Item> items = await _itemRepository.GetAsync();
        return items;
    }

    public async Task<Item> CreateAsync(SaveItemDto saveItemDto, string imagePath)
    {
        Item item = _mapper.Map<Item>(saveItemDto);
        item.ImageUrl = imagePath;
        return await _itemRepository.CreateAsync(item);
    }

    public async Task UpdateAsync(int id, SaveItemDto saveItemDto, string imagePath)
    {
        Item itemWithNewValues = _mapper.Map<Item>(saveItemDto);
        itemWithNewValues.Id = id;
        itemWithNewValues.ImageUrl = imagePath;
        Item? itemWithOldValues = await _itemRepository.GetAsync(id);

        if (itemWithOldValues == null)
        {
                throw new ApiException(HttpStatusCode.NotFound,
                    $"Didn't find any instance of entity {typeof(Item)} with ID '{id}'");
        }
        
        await _itemRepository.UpdateAsync(itemWithOldValues, itemWithNewValues);
    }

    public async Task DeleteAsync(int id)
    {
        Item? item = await _itemRepository.GetAsync(id);

        if (item != null)
        {
            await _itemRepository.DeleteAsync(item);
        }
    }

    public async Task<List<Item>> GetLowStockItemsAsync()
    {
        List<Item> lowStockItems = await _itemRepository.GetLowStockItems();
        return lowStockItems;
    }
}