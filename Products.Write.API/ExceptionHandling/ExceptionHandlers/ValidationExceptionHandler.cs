using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace Products.Write.API.ExceptionHandling.ExceptionHandlers
{
    public class ValidationExceptionHandler : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService;
        private readonly ILogger<ValidationExceptionHandler> _logger;
        public ValidationExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<ValidationExceptionHandler> logger)
        {
            _problemDetailsService = problemDetailsService;
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not ValidationException validationException) return false;   // Exception not handled

            _logger.LogWarning("Validation failed: Exception Type: {Type} | {Message} | RequestId: {RequestId}", exception.GetType().FullName, validationException.Message, httpContext.TraceIdentifier);
            _logger.LogWarning("FluentValidation error occurred with {ErrorCount} errors", validationException.Errors.Count());

            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            var problemDetails = new ValidationProblemDetails(errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Error",
                Detail = "One or more validation errors occurred.",
                Instance = httpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
            problemDetails.Extensions["timestamp"] = DateTime.UtcNow;
            problemDetails.Extensions["requestId"] = httpContext.TraceIdentifier;
            problemDetails.Extensions["machine"] = Environment.MachineName;
            // Include correlation ID if available
            problemDetails.Extensions["correlationId"] = httpContext.Request.Headers["X-Correlation-ID"].FirstOrDefault();
            problemDetails.Extensions["errors"] = errors;       // validationException.Errors;

            //// OPTION 1: HANDLE EXCEPTION AND RETURN PROBLEM DETAILS OBJECT - NOTE WILL NOT HAVE CONTENT TYPE OF `application/problem+json`
            //httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            //await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            //return true; // Exception handled

            // OPTION 2: USE PROBLEM DETAILS SERVICE TO HANDLE EXCEPTION
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
    }
}