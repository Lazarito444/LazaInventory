using LazaInventory.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LazaInventory.Infrastructure.Persistence;

public static class ServiceRegistration
{
    public static void AddPersistenceLayer(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(builder =>
        {
            builder.UseSqlServer(connectionString, optionsBuilder =>
            {
                optionsBuilder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                optionsBuilder.MigrationsHistoryTable("EF_MIGRATIONS");
            });
        });
    }
}