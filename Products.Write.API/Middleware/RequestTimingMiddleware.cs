namespace Products.Write.API.Middleware
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        public RequestTimingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            context.Items["RequestStartTime"] = DateTime.UtcNow;
            await _next(context);
        }
    }
}
