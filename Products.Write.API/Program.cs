using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Products.Write.API;
using Products.Write.API.ExceptionHandling;
using Products.Write.API.ExceptionHandling.ExceptionHandlers;
using Products.Write.API.Middleware;
using Products.Write.Application.DTOs;
using Products.Write.Domain.Enumerations;
using Scalar.AspNetCore;
using System.Security.Claims;
using System.Text.Json;
using Products.Write.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddHealthChecks()
//    .AddSqlServer(connectionString: builder.Configuration.GetConnectionString("LocalDevelopmentConnectionString")!);

// Add HealthChecks with SQL Server check
builder.Services.AddHealthChecks()
    .AddSqlServer(
        connectionString: builder.Configuration.GetConnectionString("LocalDevelopmentConnectionString")!,
        name: "sqlserver",
        failureStatus: HealthStatus.Unhealthy,
        timeout: TimeSpan.FromSeconds(5),
        tags: new[] { "db", "sql", "sqlserver" }
    );

builder.Services.AddProblemDetails(); // Registers the ProblemDetails service - configured in ExceptionHandlers using ExceptionHandlerExtensions 

// Configure Auth
JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear(); // Note: As configured, Roles are not populated by HttpContext.User.Claims without this
builder.Services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:5001";
        options.Audience = "productswriteapi";
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            NameClaimType = "given_name",       // should have the same mapping as in client app
            RoleClaimType = "role",             // should have the same mapping as in our client mvc app
            ValidTypes = new[] { "at+jwt" }     // says the only valid token type is 'at + jwt' 
            //ValidateIssuer = true,
            //ValidateAudience = true,
            //ValidateLifetime = true
        };

    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAdmin", policy => policy.RequireClaim("role", "Admin"));                          // (ClaimTypes.Role, "Admin")); does not work
    options.AddPolicy("IsManager", policy => policy.RequireClaim("role", "Manager"));                      // (ClaimTypes.Role, "Manager")); does not work
    options.AddPolicy("IsAdminOrManager", policy => policy.RequireClaim("role", "Admin", "Manager"));      // (ClaimTypes.Role, "Admin", "Manager"));does not work
    options.AddPolicy("MarlowAndWendy", policy => policy.RequireClaim(ClaimTypes.Name, "Wendy Davenport", "Marlow Bean"));
    options.AddPolicy("DomesticDogs", policy => policy.RequireClaim("Genus", "Canis").RequireClaim("Species", "Familiaris"));
});

// CONFIGURE MEDIATR AND PIPELINE BEHAVIORS
builder.Services.AddMediatR(cfg => {
    cfg.LicenseKey = builder.Configuration.GetValue<string>("MediatRSettings:LicenseKey");
    cfg.RegisterServicesFromAssembly(typeof(Products.Write.Application.DIRegistrations).Assembly);
    // Register pipeline behaviors in order
    // 1. Logging - use Serilog
    // 2. Validation - FluentValidation - change ValidationExceptionHandler to use FluentValidation.ValidationException
    // 3. Handle exceptions - use ExceptionHandlers
    // 4. Monitor performance - Serilog Request Logging
    // 5. Manage transactions
});

// Register services from Composition Root
builder.Services.ComposeApplication();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register exception handlers in order of specificity (most specific first)
builder.Services.AddExceptionHandler<ProductEventStoreExceptionHandler>();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>(); // Backup handler

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<CorrelationIdMiddleware>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// app.MapHealthChecks("/health");
app.MapHealthChecks("/api/productsManagement/health", new HealthCheckOptions
{
    // ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse

    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        //var response = new
        //{
        //    status = report.Status.ToString(),
        //    checks = report.Entries.Select(entry => new
        //    {
        //        name = entry.Key,
        //        status = entry.Value.Status.ToString(),
        //        description = entry.Value.Description,
        //        duration = entry.Value.Duration.TotalMilliseconds + "ms"
        //    }),
        //    totalDuration = report.TotalDuration.TotalMilliseconds + "ms"
        //};
        //await context.Response.WriteAsync(JsonSerializer.Serialize(response));

        HealthCheckResultDTO dto = new HealthCheckResultDTO()
        {
            Status = report.Status.ToString(),
            TotalDuration = report.TotalDuration.TotalMilliseconds + "ms"
        };

        if (report.Entries is not null && report.Entries.Any())
        {
            dto.Entries = new Dictionary<string, HealthCheckResultEntriesDTO>();
            foreach (var entry in report.Entries)
            {
                dto.Entries.Add(entry.Key, new HealthCheckResultEntriesDTO() { Status = entry.Value.Status.ToString(), Description = entry.Value.Description, Duration = entry.Value.Duration.ToString() });
            }
        }

        string jsonResult = JsonSerializer.Serialize(dto);

        if (report.Status == HealthStatus.Healthy) app.Logger.LogHealthCheckStatus(report.Status.ToString());
        //// DefaultHealthCheckService automatically logs Unhealthy result already, so no need to log error
        // else app.Logger.LogError("Health Check Result: {jsonResult}", jsonResult);

        // dev purposes only
        app.Logger.LogInformation("Health Check Result: {jsonResult}", jsonResult);

        await context.Response.WriteAsync(jsonResult);
    }

}).AllowAnonymous();        //.RequireAuthorization("IsAdminOrManager");            

app.Run();
