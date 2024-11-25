using LazaInventory.Core.Domain.Entities;

namespace LazaInventory.Core.Application.Interfaces.Repositories;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<List<Category>> GetAsync();
    Task<Category?> GetAsync(int id);
}