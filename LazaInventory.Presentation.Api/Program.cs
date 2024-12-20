using LazaInventory.Core.Application;
using LazaInventory.Infrastructure.Persistence;
using LazaInventory.Presentation.Api;
using LazaInventory.Presentation.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceLayer(builder.Configuration.GetConnectionString("AppConnection") 
                                     ?? throw new Exception("Connection String was not found"));
builder.Services.AddPresentationLayer(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseGlobalExceptionMiddleware();
app.UseHttpsRedirection();

app.Run();