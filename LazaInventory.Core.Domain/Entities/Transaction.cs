using LazaInventory.Core.Domain.Common;
using LazaInventory.Core.Domain.Enums;

namespace LazaInventory.Core.Domain.Entities;

public class Transaction : AppEntity
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public TransactionType TransactionType { get; set; }
    
    public Item Item { get; set; }
}