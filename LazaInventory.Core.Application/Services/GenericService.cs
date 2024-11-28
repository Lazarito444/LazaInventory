using LazaInventory.Core.Domain.Common;

namespace LazaInventory.Core.Application.Services;

public class GenericService<TEntity, TSaveEntityDto>
    where TEntity : AppEntity
    where TSaveEntityDto : class    
{
}