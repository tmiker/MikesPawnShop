using Microsoft.EntityFrameworkCore;
using Products.Write.API.ExceptionHandling.ExceptionHandlers;
using Products.Write.API.Middleware;
using Products.Write.Application;
using Products.Write.Infrastructure;
using Products.Write.Infrastructure.DataAccess;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<EventStoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetValue<string>("ProductEventStoreSettings:LocalDevelopmentConnectionString"));
});

builder.Services.AddProblemDetails(); // Registers the ProblemDetails service

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

builder.Services.RegisterInfrastructureServices();
builder.Services.RegisterApplicationServices();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register exception handlers in order of specificity (most specific first)
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>(); // Backup handler

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();

// Configure the HTTP request pipeline.

app.UseExceptionHandler(); // Enables the middleware to use the registered IExceptionHandler above

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle($"Pawn Shop Products Write Side API");
        options.WithTheme(ScalarTheme.Mars);
        options.EnableDarkMode();
    });
    // app.UsePathBase("/scalar/v1");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
