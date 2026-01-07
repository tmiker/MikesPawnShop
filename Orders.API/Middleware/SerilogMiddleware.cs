namespace Orders.API.Middleware
{
    public class SerilogMiddleware
    {
        private readonly RequestDelegate _next;

        public SerilogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (Serilog.Context.LogContext.PushProperty("HttpMethod", context.Request.Method))
            {
                await _next(context);
            }
        }
    }
}
