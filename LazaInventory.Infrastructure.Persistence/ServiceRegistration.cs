using LazaInventory.Core.Application.Interfaces.Repositories;
using LazaInventory.Core.Application.Interfaces.Services;
using LazaInventory.Infrastructure.Persistence.Contexts;
using LazaInventory.Infrastructure.Persistence.Entities;
using LazaInventory.Infrastructure.Persistence.Repositories;
using LazaInventory.Infrastructure.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LazaInventory.Infrastructure.Persistence;

public static class ServiceRegistration
{
    public static void AddPersistenceLayer(this IServiceCollection services, string connectionString)
    {
        // REGISTER DATABASE AND DB_CONTEXT
        services.AddDbContext<AppDbContext>(builder =>
        {
            builder.UseSqlServer(connectionString, optionsBuilder =>
            {
                optionsBuilder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                optionsBuilder.MigrationsHistoryTable("EF_MIGRATIONS");
            });
        });
        
        // REGISTER REPOSITORIES
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<IItemRepository, ItemRepository>();
        services.AddTransient<ITransactionRepository, TransactionRepository>();
        
        // IDENTITY
        services.AddIdentity<AppUser, IdentityRole<int>>(options =>
            {
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        
        // REGISTER AUTH SERVICE
        services.AddTransient<IAuthService, AuthService>();
    }
}