using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Products.Write.API.ExceptionHandling.Exceptions;

namespace Products.Write.API.ExceptionHandling.ExceptionHandlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IProblemDetailsService _problemDetailsService;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment environment, IProblemDetailsService problemDetailsService)
        {
            _logger = logger;
            _environment = environment;
            _problemDetailsService = problemDetailsService;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            // Log the exception with structured logging
            _logger.LogError(exception,
                "Exception occurred: {Message} | RequestId: {RequestId} | Path: {Path} | Method: {Method}",
                exception.Message,
                httpContext.TraceIdentifier,
                httpContext.Request.Path.Value,
                httpContext.Request.Method);

            // Create ProblemDetails based on the exception and environment - calls private method below
            var problemDetails = CreateProblemDetails(httpContext, exception);

            // Ensure response status code is set
            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;

            // Use the Microsoft.AspNetCore.Http IProblemDetailsService to write the response
            return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails,
                Exception = exception
            });
        }

        private ProblemDetails CreateProblemDetails(HttpContext httpContext, Exception exception)
        {
            // Map problem details arguments from exception type - populates tuple using private method below
            var (statusCode, title, detail, errorType) = MapException(exception);

            // Create ProblemDetails instance considering environment
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = _environment.IsDevelopment() ? exception.Message : detail,
                Type = errorType,
                Instance = httpContext.Request.Path
            };

            // Add custom extensions
            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
            problemDetails.Extensions["timestamp"] = DateTime.UtcNow;
            problemDetails.Extensions["requestId"] = httpContext.TraceIdentifier;
            // Add development-only information
            if (_environment.IsDevelopment())
            {
                problemDetails.Extensions["exceptionType"] = exception.GetType().Name;
                problemDetails.Extensions["stackTrace"] = exception.StackTrace;
            }

            // Handle validation exceptions specially
            if (exception is ValidationException validationEx)
            {
                problemDetails.Extensions["errors"] = validationEx.Errors;
            }
            return problemDetails;
        }

        private static (int statusCode, string title, string detail, string errorType) MapException(Exception exception)
        {
            return exception switch
            {
                ValidationException => (StatusCodes.Status400BadRequest,
                    "Validation Error",
                    "One or more validation errors occurred.",
                    "https://tools.ietf.org/html/rfc7231#section-6.5.1"),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized,
                    "Unauthorized",
                    "Authentication is required to access this resource.",
                    "https://tools.ietf.org/html/rfc7235#section-3.1"),
                ForbiddenException => (StatusCodes.Status403Forbidden,
                    "Forbidden",
                    "You don't have permission to access this resource.",
                    "https://tools.ietf.org/html/rfc7231#section-6.5.3"),
                NotFoundException => (StatusCodes.Status404NotFound,
                    "Resource Not Found",
                    "The requested resource was not found.",
                    "https://tools.ietf.org/html/rfc7231#section-6.5.4"),
                ConflictException => (StatusCodes.Status409Conflict,
                    "Conflict",
                    "The request could not be completed due to a conflict.",
                    "https://tools.ietf.org/html/rfc7231#section-6.5.8"),
                ArgumentException => (StatusCodes.Status400BadRequest,
                    "Bad Request",
                    "The request contains invalid arguments.",
                    "https://tools.ietf.org/html/rfc7231#section-6.5.1"),
                InvalidOperationException => (StatusCodes.Status400BadRequest,
                    "Invalid Operation",
                    "The operation is not valid for the current state.",
                    "https://tools.ietf.org/html/rfc7231#section-6.5.1"),
                TaskCanceledException => (StatusCodes.Status408RequestTimeout,
                    "Request Timeout",
                    "The request timed out.",
                    "https://tools.ietf.org/html/rfc7231#section-6.5.7"),
                _ => (StatusCodes.Status500InternalServerError,
                    "Internal Server Error",
                    "An unexpected error occurred. Please try again later.",
                    "https://tools.ietf.org/html/rfc7231#section-6.6.1")
            };
        }
    }
}
