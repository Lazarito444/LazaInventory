using System.Net;
using LazaInventory.Core.Application.Dtos.Transaction;
using LazaInventory.Core.Application.Exceptions;
using LazaInventory.Core.Application.Interfaces.Services;
using LazaInventory.Core.Domain.Entities;
using LazaInventory.Core.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace LazaInventory.Presentation.Api.Controllers.v1;

[Route("api/inventory")]
[ApiVersion("1.0")]
public class InventoryController : BaseApiController
{
    private readonly ITransactionService _transactionService;
    private readonly IItemService _itemService;

    public InventoryController(ITransactionService transactionService, IItemService itemService)
    {
        _transactionService = transactionService;
        _itemService = itemService;
    }

    [HttpGet("{id:int:required}/in")]
    public async Task<IActionResult> RegisterProductIn([FromRoute] int id, [FromQuery] int quantity)
    {
        if (id <= 0)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "The ID must be a positive integer");
        }

        if (quantity <= 0)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "The quantity must be a positive integer");
        }

        Item? item = await _itemService.GetAsync(id);

        if (item == null)
        {
            throw new ApiException(HttpStatusCode.NotFound, $"There's no Item with ID '{id}'");
        }

        await _transactionService.RegisterTransactionAsync(new SaveTransactionDto
        {
            ItemId = id,
            Quantity = quantity,
            TransactionType = TransactionType.In
        });
        
        return NoContent();
    }

    [HttpGet("{id:int:required}/out")]
    public async Task<IActionResult> RegisterProductOut([FromRoute] int id, [FromQuery] int quantity)
    {
        if (id <= 0)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "The ID must be a positive integer");
        }
        
        if (quantity <= 0)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "The quantity must be a positive integer");
        }
        
        Item? item = await _itemService.GetAsync(id);

        if (item == null)
        {
            throw new ApiException(HttpStatusCode.NotFound, $"There's no Item with ID '{id}'");
        }
        
        await _transactionService.RegisterTransactionAsync(new SaveTransactionDto
        {
            ItemId = id,
            Quantity = quantity,
            TransactionType = TransactionType.Out
        });

        return NoContent();
    }
}