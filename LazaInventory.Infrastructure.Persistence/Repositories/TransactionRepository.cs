using LazaInventory.Core.Application.Interfaces.Repositories;
using LazaInventory.Core.Domain.Entities;
using LazaInventory.Infrastructure.Persistence.Contexts;

namespace LazaInventory.Infrastructure.Persistence.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _dbContext;
    
    public TransactionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Transaction> RegisterTransactionAsync(Transaction transaction)
    {
        await _dbContext.Set<Transaction>().AddAsync(transaction);
        await _dbContext.SaveChangesAsync();
        return transaction;
    }
}