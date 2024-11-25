using System.Net;
using System.Text.Json;
using LazaInventory.Core.Application.Exceptions;

namespace LazaInventory.Presentation.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

        ErrorDetails errorDetails = new ErrorDetails
        {
            ExceptionType = HttpStatusCode.InternalServerError.ToString(),
            ExceptionMessage = "Something went wrong..."
        };
        
        if (exception is ApiException apiException)
        {
            statusCode = apiException.StatusCode;
            errorDetails.ExceptionType = statusCode.ToString();
            errorDetails.ExceptionMessage = apiException.ExceptionMessage;
        }

        context.Response.StatusCode = (int) statusCode;
        string response = JsonSerializer.Serialize(errorDetails);
        return context.Response.WriteAsync(response);
    }
}

public class ErrorDetails
{
    public string ExceptionType { get; set; }
    public string ExceptionMessage { get; set; }
}