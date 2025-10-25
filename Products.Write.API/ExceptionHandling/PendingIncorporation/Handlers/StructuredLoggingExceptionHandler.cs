using Microsoft.AspNetCore.Diagnostics;
using Products.Write.API.ExceptionHandling.Exceptions;
using Products.Write.API.ExceptionHandling.PendingIncorporation.Services;

namespace Products.Write.API.ExceptionHandling.PendingIncorporation.Handlers
{
    public class StructuredLoggingExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<StructuredLoggingExceptionHandler> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IProblemDetailsService _problemDetailsService;

        public StructuredLoggingExceptionHandler(ILogger<StructuredLoggingExceptionHandler> logger, IWebHostEnvironment environment, IProblemDetailsService problemDetailsService)
        {
            _logger = logger;
            _environment = environment;
            _problemDetailsService = problemDetailsService;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            // Create structured log data
            using var scope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["RequestId"] = httpContext.TraceIdentifier,
                ["RequestPath"] = httpContext.Request.Path.Value ?? string.Empty,
                ["RequestMethod"] = httpContext.Request.Method,
                ["UserAgent"] = httpContext.Request.Headers["User-Agent"].FirstOrDefault() ?? string.Empty,
                ["RemoteIpAddress"] = httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                ["UserId"] = httpContext.User?.Identity?.Name ?? "Anonymous",
                ["CorrelationId"] = httpContext.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? string.Empty
            });

            // Log based on exception severity
            var logLevel = DetermineLogLevel(exception);
            _logger.Log(logLevel, exception,
                "Unhandled exception occurred: {ExceptionType} - {ExceptionMessage}",
                exception.GetType().Name, exception.Message);

            // Additional metrics logging
            LogExceptionMetrics(httpContext, exception);
            var problemDetails = ProblemDetailsFactory.CreateProblemDetails(
                httpContext, exception, _environment.IsDevelopment());
            httpContext.Response.StatusCode = problemDetails.Status ?? 500;
            return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails,
                Exception = exception
            });
        }
        private static LogLevel DetermineLogLevel(Exception exception)
        {
            return exception switch
            {
                ValidationException => LogLevel.Warning,
                NotFoundException => LogLevel.Warning,
                UnauthorizedAccessException => LogLevel.Warning,
                ForbiddenException => LogLevel.Warning,
                ConflictException => LogLevel.Information,
                ArgumentException => LogLevel.Warning,
                _ => LogLevel.Error
            };
        }
        private void LogExceptionMetrics(HttpContext httpContext, Exception exception)
        {
            // Log additional metrics for monitoring
            _logger.LogInformation("Exception metrics: {ExceptionType} | Status: {StatusCode} | Duration: {Duration}ms",
                exception.GetType().Name,
                GetStatusCodeFromException(exception),
                GetRequestDuration(httpContext));
        }
        private static int GetStatusCodeFromException(Exception exception)
        {
            return exception switch
            {
                ValidationException => 400,
                NotFoundException => 404,
                UnauthorizedAccessException => 401,
                ForbiddenException => 403,
                ConflictException => 409,
                _ => 500
            };
        }
        private static long GetRequestDuration(HttpContext httpContext)
        {
            if (httpContext.Items.TryGetValue("RequestStartTime", out var startTimeObj)
                && startTimeObj is DateTime startTime)
            {
                return (long)(DateTime.UtcNow - startTime).TotalMilliseconds;
            }
            return 0;
        }
    }
}
