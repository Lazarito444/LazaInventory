using LazaInventory.Core.Domain.Common;

namespace LazaInventory.Core.Domain.Entities;

public class Item : AppEntity
{
    public int Stock { get; set; }
    public int MinimumStock { get; set; }
    public int CategoryId { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    
    public Category Category { get; set; }
}