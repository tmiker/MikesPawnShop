using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Products.Write.Infrastructure.Exceptions;

namespace Products.Write.API.ExceptionHandling.ExceptionHandlers
{
    public class ProductEventStoreExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ProductEventStoreExceptionHandler> _logger;
        public ProductEventStoreExceptionHandler(ILogger<ProductEventStoreExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not ProductEventStoreException productEventStoreException) return false; // Exception not handled

            _logger.LogWarning("Resource not found: {Message} | RequestId: {RequestId}", productEventStoreException.Message, httpContext.TraceIdentifier);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource Not Found",
                Detail = productEventStoreException.Message,
                Instance = httpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
            };

            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
            problemDetails.Extensions["timestamp"] = DateTime.UtcNow;
            problemDetails.Extensions["requestId"] = httpContext.TraceIdentifier;
            problemDetails.Extensions["machine"] = Environment.MachineName;
            // Include correlation ID if available
            problemDetails.Extensions["correlationId"] = httpContext.Request.Headers["X-Correlation-ID"].FirstOrDefault();

            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true; // Exception handled
        }
    }
}