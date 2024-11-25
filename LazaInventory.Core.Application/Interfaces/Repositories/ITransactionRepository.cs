using LazaInventory.Core.Domain.Entities;

namespace LazaInventory.Core.Application.Interfaces.Repositories;

public interface ITransactionRepository
{
    Task<Transaction> RegisterTransactionAsync(Transaction transaction);
}