using System.Net;
using LazaInventory.Core.Application.Dtos.Category;
using LazaInventory.Core.Application.Exceptions;
using LazaInventory.Core.Application.Interfaces.Services;
using LazaInventory.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LazaInventory.Presentation.Api.Controllers.v1;

[Route("api/categories")]
public class CategoryController : BaseApiController
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCategories()
    {
        List<Category> categories = await _categoryService.GetAsync();

        if (categories.Count == 0) return NoContent();
        
        return Ok(categories);
    }

    [HttpGet("{id:int:required}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCategory(int id)
    {
        if (id <= 0)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "The ID must be a positive integer");
        }
        
        Category? category = await _categoryService.GetAsync(id);

        if (category == null)
        {
            throw new ApiException(HttpStatusCode.NotFound, $"There's no category with ID '{id}'");
        }

        return Ok(category);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostCategory([FromBody] SaveCategoryDto saveCategoryDto)
    {
        if (!ModelState.IsValid)
        {
            string exceptionMessage = "Errors: ";
            foreach (KeyValuePair<string, ModelStateEntry> entry in ModelState)
            {
                foreach (ModelError error in entry.Value.Errors)
                {
                    exceptionMessage += $"{error.ErrorMessage}. ";
                }
            }

            throw new ApiException(HttpStatusCode.BadRequest, exceptionMessage);
        }
        
        Category category = await _categoryService.CreateAsync(saveCategoryDto);
        return CreatedAtAction(nameof(PostCategory), new { category.Id }, category);
    }

    [HttpDelete("{id:int:required}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        if (id <= 0)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "The ID must be a positive integer");
        }
        
        await _categoryService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("{id:int:required}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] SaveCategoryDto saveCategoryDto)
    {      
        if (id <= 0)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "The ID must be a positive integer");
        }
        
        if (!ModelState.IsValid)
        {
            string exceptionMessage = String.Empty;
            foreach (KeyValuePair<string, ModelStateEntry> entry in ModelState)
            {
                foreach (ModelError error in entry.Value.Errors)
                {
                    exceptionMessage += $"{error.ErrorMessage} | ";
                }
            }

            throw new ApiException(HttpStatusCode.BadRequest, exceptionMessage);
        }
        
        await _categoryService.UpdateAsync(id, saveCategoryDto);
        return NoContent();
    }
}