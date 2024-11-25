using LazaInventory.Core.Application.Interfaces.Repositories;
using LazaInventory.Core.Domain.Entities;
using LazaInventory.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LazaInventory.Infrastructure.Persistence.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    private readonly AppDbContext _dbContext;
    public CategoryRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Category>> GetAsync()
    {
        List<Category> categories = await _dbContext.Set<Category>().ToListAsync();
        return categories;
    }
    
    public async Task<Category?> GetAsync(int id)
    {
        Category? category = await _dbContext.Set<Category>().FindAsync(id);
        return category;
    }
}