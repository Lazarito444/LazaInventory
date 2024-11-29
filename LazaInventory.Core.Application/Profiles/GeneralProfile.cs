using AutoMapper;
using LazaInventory.Core.Application.Dtos.Category;
using LazaInventory.Core.Application.Dtos.Item;
using LazaInventory.Core.Application.Dtos.Transaction;
using LazaInventory.Core.Domain.Entities;

namespace LazaInventory.Core.Application.Profiles;

public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        // ITEM
        CreateMap<Item, SaveItemDto>().ReverseMap();

        // CATEGORY
        CreateMap<Category, SaveCategoryDto>().ReverseMap();
        
        // TRANSACTION
        CreateMap<Transaction, SaveTransactionDto>().ReverseMap();
    }
}