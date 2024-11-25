using System.Net;

namespace LazaInventory.Core.Application.Exceptions;

public class ApiException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string ExceptionMessage { get; }
    
    public ApiException(HttpStatusCode statusCode, string exceptionMessage)
    {
        StatusCode = statusCode;
        ExceptionMessage = exceptionMessage;
    }
}