using Products.Write.API.ExceptionHandling.PendingIncorporation.Models;
using Products.Write.Application.Exceptions;

namespace Products.Write.API.ExceptionHandling.PendingIncorporation.Services
{
    public class ProblemDetailsFactory
    {
        public static ApiProblemDetails CreateProblemDetails(
        HttpContext context,
        Exception exception,
        bool isDevelopment = false)
        {
            var (statusCode, title, detail, type) = MapException(exception);

            var problemDetails = new ApiProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = isDevelopment ? exception.Message : detail,
                Type = type,
                Instance = context.Request.Path.Value,
                TraceId = context.TraceIdentifier,
                Timestamp = DateTime.UtcNow,
                CorrelationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
            };
            // Add context information
            problemDetails.Context = new Dictionary<string, object>
            {
                ["method"] = context.Request.Method,
                ["path"] = context.Request.Path.Value ?? string.Empty,
                ["queryString"] = context.Request.QueryString.Value ?? string.Empty,
                ["userAgent"] = context.Request.Headers["User-Agent"].FirstOrDefault() ?? string.Empty
            };
            // Handle validation exceptions
            if (exception is ValidationException validationEx)
            {
                problemDetails.Errors = validationEx.Errors.SelectMany(kvp =>
                    kvp.Value.Select(error => new ApiError
                    {
                        Code = "VALIDATION_ERROR",
                        Description = error,
                        Field = kvp.Key
                    })).ToList();
            }
            // Add development information
            if (isDevelopment)
            {
                problemDetails.Context["exceptionType"] = exception.GetType().FullName ?? string.Empty;
                problemDetails.Context["stackTrace"] = exception.StackTrace ?? string.Empty;
            }
            return problemDetails;
        }
        private static (int statusCode, string title, string detail, string type) MapException(Exception exception)
        {
            return exception switch
            {
                ValidationException => (400, "Validation Error",
                    "One or more validation errors occurred.",
                    "https://httpstatuses.com/400"),

                NotFoundException => (404, "Resource Not Found",
                    "The requested resource was not found.",
                    "https://httpstatuses.com/404"),

                UnauthorizedAccessException => (401, "Unauthorized",
                    "Authentication is required to access this resource.",
                    "https://httpstatuses.com/401"),

                ForbiddenException => (403, "Forbidden",
                    "You don't have permission to access this resource.",
                    "https://httpstatuses.com/403"),

                ConflictException => (409, "Conflict",
                    "The request could not be completed due to a conflict.",
                    "https://httpstatuses.com/409"),

                _ => (500, "Internal Server Error",
                    "An unexpected error occurred.",
                    "https://httpstatuses.com/500")
            };
        }
    }
}
