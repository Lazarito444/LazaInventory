using LazaInventory.Core.Domain.Enums;

namespace LazaInventory.Core.Application.Dtos.Transaction;

public class SaveTransactionDto
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public TransactionType TransactionType { get; set; }
}