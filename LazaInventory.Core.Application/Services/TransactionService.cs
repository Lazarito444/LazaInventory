using AutoMapper;
using LazaInventory.Core.Application.Dtos.Transaction;
using LazaInventory.Core.Application.Interfaces.Repositories;
using LazaInventory.Core.Application.Interfaces.Services;
using LazaInventory.Core.Domain.Entities;

namespace LazaInventory.Core.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;

    public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task RegisterTransactionAsync(SaveTransactionDto saveTransactionDto)
    {
        Transaction transaction = _mapper.Map<Transaction>(saveTransactionDto);
        await _transactionRepository.RegisterTransactionAsync(transaction);
    }
}