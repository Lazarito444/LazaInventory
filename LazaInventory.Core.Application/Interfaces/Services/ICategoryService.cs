using LazaInventory.Core.Application.Dtos.Category;
using LazaInventory.Core.Domain.Entities;

namespace LazaInventory.Core.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<List<Category>> GetAsync();
    Task<Category?> GetAsync(int id);
    Task<Category> CreateAsync(SaveCategoryDto saveCategoryDto);
    Task DeleteAsync(int id);
    Task UpdateAsync(int id, SaveCategoryDto saveCategoryDto);
}