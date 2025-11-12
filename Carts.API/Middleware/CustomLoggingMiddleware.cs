namespace Carts.API.Middleware
{
    public class CustomLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomLoggingMiddleware> _logger;

        public CustomLoggingMiddleware(RequestDelegate next, ILogger<CustomLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("Request: {method} {path} ", context.Request.Method, context.Request.Path);
            await _next(context);
            _logger.LogInformation("Response: {status code}", context.Response.StatusCode);
        }
    }
}
