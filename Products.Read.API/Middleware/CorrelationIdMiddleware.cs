namespace Products.Read.API.Middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CorrelationIdHeader = "X-Correlation-ID";

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //public async Task InvokeAsync(HttpContext context)
        //{
        //    Console.WriteLine("************ CORRELATION ID MIDDLEWARE WAS CALLED **************");

        //    if (!context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId))
        //    {
        //        correlationId = Guid.NewGuid().ToString();
        //    }

        //    context.Response.OnStarting(() =>
        //    {
        //        context.Response.Headers.Append(CorrelationIdHeader, correlationId.ToString());
        //        return Task.CompletedTask;
        //    });

        //    await _next(context);
        //}

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine("************ CORRELATION ID MIDDLEWARE WAS CALLED **************");

            // Check if Correlation ID exists in the request header
            if (!context.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
            {
                // Generate a new Correlation ID if not present
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers["X-Correlation-ID"] = correlationId;
            }

            // Add Correlation ID to the response header
            context.Response.Headers["X-Correlation-ID"] = correlationId;

            // Proceed to the next middleware
            await _next(context);
        }
    }
}
