using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Products.Write.API;
using Products.Write.API.ExceptionHandling;
using Products.Write.API.ExceptionHandling.ExceptionHandlers;
using Products.Write.API.Middleware;
using Scalar.AspNetCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString: builder.Configuration.GetConnectionString("LocalDevelopmentConnectionString")!);

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
    cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzg4MzkzNjAwIiwiaWF0IjoiMTc1NjkzMjY4NiIsImFjY291bnRfaWQiOiIwMTk5MTE1ODc1OWE3ZDA3OGU3NTE0ZTAxMWI4MzQ4NyIsImN1c3RvbWVyX2lkIjoiY3RtXzAxazQ4bmpnbnJnMjZzdmRnYWtqNjQzcXNlIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.jmdKYTTTsbyPp4zI-pVr4T4jsYwg-M6qJHHBtCCCKXGP_B7duOiEvzFvKkPKNGZ2830BtsJdewfithYQDQawRVVV-MZa1bPa27AiMgMj5H9HDBN84h5XbRszjGhLMxgklMQ1HTFDOBGpJDANE3UwgdM0OJywJfVc2n67lZ-w6_y8W-5_xQGIbBlqVFcYTf3tk5Y2cpE2kPJY0li6BH6W2aBKNSXyvtE8jpxPr5Y2W7gqqHerLWWSPQkNlgm8QGJYXpPJg4EgHvmElM8HUccNfuh-JfCQ2w7hyjkb-MEEIXjaGQ8uCiYaiaGvfQ0sorxrEgUQBQTzS2rdkEn-Gb6TXA";
    cfg.RegisterServicesFromAssembly(typeof(Products.Write.Application.DIRegistrations).Assembly);
    // Register pipeline behaviors in order
    // 1. Log everything
    // 2. Validate early
    // 3. Handle exceptions
    // 4. Monitor performance
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
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}).AllowAnonymous();        // .RequireAuthorization("IsAdminOrManager");

app.Run();
