using Products.Read.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//// ProblemDetails service - configure globally if not using ExceptionHandlers
//builder.Services.AddProblemDetails(options =>
//{
//    // Customize problem details globally
//    options.CustomizeProblemDetails = (context) =>
//    {
//        context.ProblemDetails.Extensions["machine"] = Environment.MachineName;
//        context.ProblemDetails.Extensions["requestId"] = context.HttpContext.TraceIdentifier;
//        // Add correlation ID if available
//        if (context.HttpContext.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
//        {
//            context.ProblemDetails.Extensions["correlationId"] = correlationId.ToString();
//        }
//    };
//});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
