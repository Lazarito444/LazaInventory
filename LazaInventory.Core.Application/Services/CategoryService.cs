using System.Net;
using AutoMapper;
using LazaInventory.Core.Application.Dtos.Category;
using LazaInventory.Core.Application.Exceptions;
using LazaInventory.Core.Application.Interfaces.Repositories;
using LazaInventory.Core.Application.Interfaces.Services;
using LazaInventory.Core.Domain.Entities;

namespace LazaInventory.Core.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<List<Category>> GetAsync()
    {
        List<Category> categories = await _categoryRepository.GetAsync();
        return categories;
    }

    public async Task<Category?> GetAsync(int id)
    {
        Category? category = await _categoryRepository.GetAsync(id);
        return category;
    }
    
    public async Task<Category> CreateAsync(SaveCategoryDto saveCategoryDto)
    {
        Category category = _mapper.Map<Category>(saveCategoryDto);
        return await _categoryRepository.CreateAsync(category); 
    }

    public async Task DeleteAsync(int id)
    {
        Category? category = await _categoryRepository.GetAsync(id);

        if (category == null)
        {
            throw new ApiException(HttpStatusCode.NotFound,
                $"Didn't find any instance of entity {typeof(Category)} with ID '{id}'");
        }
        await _categoryRepository.DeleteAsync(category);
    }

    public async Task UpdateAsync(int id, SaveCategoryDto saveCategoryDto)
    {
        Category categoryWithNewValues = _mapper.Map<Category>(saveCategoryDto);
        Category? categoryWithOldValues = await _categoryRepository.GetAsync(id);
        
        if (categoryWithOldValues == null)
        {
            throw new ApiException(HttpStatusCode.NotFound,
                $"Didn't find any instance of entity {typeof(Category)} with ID '{id}'");
        }        
        categoryWithNewValues.Id = id;
        await _categoryRepository.UpdateAsync(categoryWithOldValues, categoryWithNewValues);
    }
}