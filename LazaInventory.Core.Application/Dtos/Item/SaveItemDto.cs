using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LazaInventory.Core.Application.Dtos.Item;

public class SaveItemDto
{
    [Required(ErrorMessage = "Stock is required"), Range(0, double.MaxValue, ErrorMessage = "Wrong quantity")]
    public int Stock { get; set; }
    
    [Required(ErrorMessage = "Minimum Stock is required"), Range(0, double.MaxValue, ErrorMessage = "Wrong quantity")]
    public int MinimumStock { get; set; }
    
    [Required(ErrorMessage = "Category ID is required")]
    public int CategoryId { get; set; }
    
    [Required(ErrorMessage = "Price is required"), Range(0, double.MaxValue, ErrorMessage = "Wrong price")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "Name is required"), DataType(DataType.Text)]
    public string Name { get; set; }
    
    [DataType(DataType.Upload)]
    public IFormFile Image { get; set; }
}