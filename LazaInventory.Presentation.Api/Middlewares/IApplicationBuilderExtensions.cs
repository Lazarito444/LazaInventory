namespace LazaInventory.Presentation.Api.Middlewares;

public static class IApplicationBuilderExtensions
{
    public static void UseGlobalExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}