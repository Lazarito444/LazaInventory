using System.Reflection;
using LazaInventory.Core.Application.Interfaces.Services;
using LazaInventory.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LazaInventory.Core.Application;

public static class ServiceRegistration
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        // REGISTER AUTOMAPPER
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // REGISTER SERVICES CLASSES
        services.AddTransient<ICategoryService, CategoryService>();
    }
}