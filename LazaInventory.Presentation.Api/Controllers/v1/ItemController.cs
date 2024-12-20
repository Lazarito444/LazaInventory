using System.Net;
using LazaInventory.Core.Application.Dtos.Item;
using LazaInventory.Core.Application.Dtos.Transaction;
using LazaInventory.Core.Application.Exceptions;
using LazaInventory.Core.Application.Interfaces.Services;
using LazaInventory.Core.Domain.Entities;
using LazaInventory.Core.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LazaInventory.Presentation.Api.Controllers.v1;

[Route("api/items")]
[ApiVersion("1.0")]
[Authorize]
public class ItemController : BaseApiController
{
    private readonly IItemService _itemService;
    private readonly ITransactionService _transactionService;
    public ItemController(IItemService itemService, ITransactionService transactionService)
    {
        _itemService = itemService;
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetItems()
    {
        List<Item> items = await _itemService.GetAsync();

        if (items.Count == 0) return NoContent();

        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetItem([FromRoute] int id)
    {
        if (id <= 0)
        {
            string exceptionMessage = $"The ID must be a positive integer.";
            throw new ApiException(HttpStatusCode.BadRequest, exceptionMessage);
        }
        
        Item? item = await _itemService.GetAsync(id);

        if (item == null)
        {
            string exceptionMessage = $"There's no item with ID '{id}'.";
            throw new ApiException(HttpStatusCode.NotFound, exceptionMessage);
        }

        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> CreateItem([FromForm] SaveItemDto saveItemDto)
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

        string imagePath = await SaveImageAsync(saveItemDto.Image);
        Item newItem = await _itemService.CreateAsync(saveItemDto, imagePath);
        await _transactionService.RegisterTransactionAsync(new SaveTransactionDto
        {
            Quantity = newItem.Stock,
            ItemId = newItem.Id,
            TransactionType = TransactionType.In
        });
        
        return CreatedAtAction(nameof(GetItem), new { newItem.Id }, newItem);
    }

    [HttpDelete("{id:int:required}")]
    public async Task<IActionResult> DeleteItem([FromRoute] int id)
    {
        if (id <= 0)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "The ID must be a positive integer");
        }

        await _itemService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("{id:int:required}")]
    public async Task<IActionResult> UpdateItem([FromForm] UpdateItemDto updateItemDto, [FromRoute] int id)
    {
        Item? item = await _itemService.GetAsync(id);

        if (item == null)
        {
            throw new ApiException(HttpStatusCode.NotFound, $"There's no item with ID '{id}'");
        }

        await UpdateImageAsync(updateItemDto.Image, item.ImageUrl);
        await _itemService.UpdateAsync(id, updateItemDto, item.ImageUrl);
        
        return NoContent();
    }

    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStockItems()
    {
        List<Item> lowStockItems = await _itemService.GetLowStockItemsAsync();

        if (lowStockItems.Count == 0) return NoContent();

        return Ok(lowStockItems);
    }
    
    private async Task<string> SaveImageAsync(IFormFile image)
    {
        string[] allowedExtensions = [".jpg", ".jpeg", ".png"];
        string imgExtension = Path.GetExtension(image.FileName);

        if (!allowedExtensions.Contains(imgExtension))
        {
            string exceptionMessage = "The extension of the image must be one of these: .jpg | .jpeg | .png";
            throw new ApiException(HttpStatusCode.BadRequest, exceptionMessage);
        }

        string wwwrootPath = "wwwroot";
        string imgUploadsPath = Path.Combine(wwwrootPath, "imguploads");
        
        if (!Directory.Exists(wwwrootPath))
        {
            Directory.CreateDirectory(wwwrootPath);
        }
        
        if (!Directory.Exists(imgUploadsPath))
        {
            Directory.CreateDirectory(imgUploadsPath);
        }

        string fileName = $"{Guid.NewGuid().ToString()}{imgExtension}";
        string filePath = Path.Combine(imgUploadsPath, fileName);

        await using FileStream stream = new FileStream(filePath, FileMode.Create);
        await image.CopyToAsync(stream);

        return filePath;
    }

    private async Task UpdateImageAsync(IFormFile image, string oldImagePath)
    {
        string[] allowedExtensions = [".jpg", ".jpeg", ".png"];
        string imgExtension = Path.GetExtension(image.FileName);

        if (!allowedExtensions.Contains(imgExtension))
        {
            string exceptionMessage = "The extension of the image must be one of these: .jpg | .jpeg | .png";
            throw new ApiException(HttpStatusCode.BadRequest, exceptionMessage);
        }

        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }

        await using FileStream stream = new FileStream(oldImagePath, FileMode.Create);
        await image.CopyToAsync(stream);
    }
}