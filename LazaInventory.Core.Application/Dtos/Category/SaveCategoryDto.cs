using System.ComponentModel.DataAnnotations;

namespace LazaInventory.Core.Application.Dtos.Category;

public class SaveCategoryDto
{
    [Required(ErrorMessage = "Name is required"), DataType(DataType.Text)]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Description is required"), DataType(DataType.Text)]
    public string Description { get; set; }
}