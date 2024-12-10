using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;

namespace LazaInventory.Presentation.Api;

public static class ServiceRegistration
{
    public static void AddPresentationLayer(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo 
            {
                Version = "v1",
                Title = "LazaInventory API",
                Description = "This API provide a way to create transactions, items and categories, you can also read, update and delete items and categories.",
                Contact = new OpenApiContact
                {
                    Name = "Ariel David Lázaro Pérez (@Lazarito444)",
                    Email = "ariellazaro444@gmail.com",
                    Url = new Uri("https://github.com/Lazarito444")
                }
            });
            options.DescribeAllParametersInCamelCase();
            string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                }
            );
        });
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = new HeaderApiVersionReader("X-version");
            options.ReportApiVersions = true;
        });
    }
}