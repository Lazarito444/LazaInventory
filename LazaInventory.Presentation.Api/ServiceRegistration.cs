using System.Reflection;
using System.Text;
using LazaInventory.Core.Domain.Options;
using LazaInventory.Presentation.Api.Converters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace LazaInventory.Presentation.Api;

public static class ServiceRegistration
{
    public static void AddPresentationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        JWTOptions jwtOptions = new JWTOptions();
        configuration.GetSection(JWTOptions.JWTSettings).Bind(jwtOptions);

        services.Configure<JWTOptions>(configuration.GetSection(JWTOptions.JWTSettings));
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
            });
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
            
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Name = "Authorization",
                Description = "Input your Bearer token in this format: Bearer <your token here>"
            });
            
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        },
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        Name = JwtBearerDefaults.AuthenticationScheme,
                        In = ParameterLocation.Header
                    }, 
                    new List<string>()
                }
            });
            
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

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
            };
        });
    }
}