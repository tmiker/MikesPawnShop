using Products.Read.API;
using Products.Read.API.Middleware;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddProblemDetails();
//// ProblemDetails service - configure globally if not using extensions in middleware
//services.AddProblemDetails(options =>
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

// Register services from Composition Root
builder.Services.ComposeApplication();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle($"Pawn Shop Products Read Side API");
        options.WithTheme(ScalarTheme.DeepSpace);
        options.EnableDarkMode();
    });
    // app.UsePathBase("/scalar/v1");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
