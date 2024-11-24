using LazaInventory.Core.Domain.Common;

namespace LazaInventory.Core.Domain.Entities;

public class Category : AppEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    public ICollection<Item> Items { get; set; }
}