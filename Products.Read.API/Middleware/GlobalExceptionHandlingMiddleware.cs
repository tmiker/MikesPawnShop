using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Products.Read.API.Exceptions;
using System.Text.Json;

namespace Products.Read.API.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;
        public GlobalExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlingMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred. RequestId: {RequestId}, Path: {Path}",
                context.TraceIdentifier, context.Request.Path);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";
            var problemDetails = CreateProblemDetails(context, exception);

            context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var json = JsonSerializer.Serialize(problemDetails, options);
            await context.Response.WriteAsync(json);
        }

        private ProblemDetails CreateProblemDetails(HttpContext context, Exception exception)
        {
            var (statusCode, title, detail) = MapException(exception);
            return new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = _environment.IsDevelopment() ? exception.Message : detail,
                Instance = context.Request.Path,
                Type = $"https://httpstatuses.com/{statusCode}",
                Extensions = new Dictionary<string, object?>
                {
                    ["traceId"] = context.TraceIdentifier,
                    ["timestamp"] = DateTime.UtcNow,
                    ["requestId"] = context.TraceIdentifier,
                    ["machine"] = Environment.MachineName,
                    ["exception"] = _environment.IsDevelopment() ? exception.GetType().Name : null,
                    ["correlationId"] = context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
                }
            };
        }

        private static (int statusCode, string title, string detail) MapException(Exception exception)
        {
            return exception switch
            {
                DataConsistencyException => (StatusCodes.Status422UnprocessableEntity,
                    "Data Synchronization Error", "Write Side Data Conflict.  Please Contact Support Immediately."),

                DbUpdateException => (StatusCodes.Status422UnprocessableEntity,
                    "Database Update Exception", "Possible Read Side Data Corruption. Please Contact Support Immediately."),

                ValidationException => (StatusCodes.Status400BadRequest,
                    "Validation Error", "One or more validation errors occurred."),

                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized,
                    "Unauthorized", "Authentication is required to access this resource."),

                ForbiddenException => (StatusCodes.Status403Forbidden,
                    "Forbidden", "You don't have permission to access this resource."),

                NotFoundException => (StatusCodes.Status404NotFound,
                    "Resource Not Found", "The requested resource was not found."),

                ConflictException => (StatusCodes.Status409Conflict,
                    "Conflict", "The request could not be completed due to a conflict."),

                ArgumentNullException => (StatusCodes.Status400BadRequest,
                    "Argument Null", exception.Message),

                ArgumentException => (StatusCodes.Status400BadRequest,
                    "Bad Request", "The request contains invalid arguments."),


                InvalidOperationException => (StatusCodes.Status400BadRequest,
                    "Invalid Operation", "The operation is not valid for the current state."),

                TimeoutException => (StatusCodes.Status408RequestTimeout,
                    "Request Timeout", "The request timed out."),

                _ => (StatusCodes.Status500InternalServerError,
                    "Internal Server Error", "An unexpected error occurred.")
            };
        }
    }
}
