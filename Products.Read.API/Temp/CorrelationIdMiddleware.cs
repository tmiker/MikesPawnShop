namespace Products.Write.API.Middleware
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

/*
To pass a Correlation ID to an API, you can use it as a unique identifier to track requests across different services or components. Here's how you can implement it in a .NET Core application:

1. Generate and Pass Correlation ID in Middleware
You can create middleware to generate or retrieve a Correlation ID from incoming requests and ensure it is passed along with outgoing requests.


public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
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
2. Register Middleware in Startup
Add the middleware to your application pipeline in the Program.cs or Startup.cs file.

Csharp

Copy code
public class Startup
{
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
3. Pass Correlation ID to Outgoing API Calls
When making HTTP requests to other APIs, ensure the Correlation ID is included in the headers.


public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<HttpResponseMessage> GetAsync(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);

        // Retrieve Correlation ID from the current context
        if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
        {
            request.Headers.Add("X-Correlation-ID", correlationId.ToString());
        }

        return await _httpClient.SendAsync(request);
    }
}
4. Add Correlation ID to Logs
To improve observability, include the Correlation ID in your logs.

public class LoggingService
{
    private readonly ILogger<LoggingService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoggingService(ILogger<LoggingService> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public void LogInformation(string message)
    {
        var correlationId = _httpContextAccessor.HttpContext?.Request.Headers["X-Correlation-ID"];
        _logger.LogInformation($"Correlation ID: {correlationId} - {message}");
    }
}
This approach ensures that the Correlation ID is consistently passed across requests, responses, and logs, enabling better traceability and debugging in distributed systems.
*/