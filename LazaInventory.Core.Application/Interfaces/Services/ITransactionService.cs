using LazaInventory.Core.Application.Dtos.Transaction;

namespace LazaInventory.Core.Application.Interfaces.Services;

public interface ITransactionService
{
    Task RegisterTransactionAsync(SaveTransactionDto saveTransactionDto);
}