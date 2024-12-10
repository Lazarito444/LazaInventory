using AutoMapper;
using LazaInventory.Core.Application.Dtos.Transaction;
using LazaInventory.Core.Application.Interfaces.Repositories;
using LazaInventory.Core.Application.Interfaces.Services;
using LazaInventory.Core.Domain.Entities;
using LazaInventory.Core.Domain.Enums;

namespace LazaInventory.Core.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IMapper _mapper;

    public TransactionService(ITransactionRepository transactionRepository, IMapper mapper, IItemRepository itemRepository)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _itemRepository = itemRepository;
    }

    public async Task RegisterTransactionAsync(SaveTransactionDto saveTransactionDto)
    {
        Transaction transaction = _mapper.Map<Transaction>(saveTransactionDto);

        if (saveTransactionDto.TransactionType == TransactionType.In)
        {
            await _itemRepository.AddStockToItemAsync(saveTransactionDto.ItemId, saveTransactionDto.Quantity);
        }
        else
        {
            await _itemRepository.RemoveStockToItemAsync(saveTransactionDto.ItemId, saveTransactionDto.Quantity);

        }
        await _transactionRepository.RegisterTransactionAsync(transaction);
    }
}