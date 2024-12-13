using LazaInventory.Core.Application;
using LazaInventory.Infrastructure.Persistence;
using LazaInventory.Presentation.GraphQL;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceLayer(builder.Configuration.GetConnectionString("AppConnection")!);
builder.Services.AddGraphQLLayer();

WebApplication app = builder.Build();

app.MapGraphQL();

app.Run();