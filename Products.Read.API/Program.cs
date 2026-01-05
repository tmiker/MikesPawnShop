using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Products.Read.API;
using Products.Read.API.DTOs;
using Products.Read.API.Extensions;
using Products.Read.API.Health;
using Products.Read.API.Middleware;
using Scalar.AspNetCore;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddHealthChecks()
//    .AddSqlServer(builder.Configuration.GetConnectionString("LocalDevelopmentConnectionString")!);

// Add HealthChecks with SQL Server check
builder.Services.AddHealthChecks()
    .AddSqlServer(
        connectionString: builder.Configuration.GetConnectionString("LocalDevelopmentConnectionString")!,
        name: "sqlserver",
        failureStatus: HealthStatus.Unhealthy,
        timeout: TimeSpan.FromSeconds(5),
        tags: new[] { "db", "sql", "sqlserver" }
    );

//builder.Services.AddHealthChecks()
//    // Add a health check for a SQL Server database
//    .AddCheck(
//        name: "SqlServer",
//        instance: new SqlServerHealthCheck(builder.Configuration.GetConnectionString("LocalDevelopmentConnectionString")!),
//        failureStatus: HealthStatus.Unhealthy,
//        tags: new string[] { "sql", "sqlserver" });

builder.Services.AddCors(setup =>
{
    setup.AddPolicy("AllowGetPolicy", policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.WithMethods("GET");
        policy.WithExposedHeaders("X-Pagination");
    });
});

builder.Services.AddProblemDetails();

// Configure Auth
JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear(); // Note: As configured, Roles are not populated by HttpContext.User.Claims without this
builder.Services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:5001";
        options.Audience = "productsreadapi";
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

/// RESPONSE CACHING
builder.Services.AddResponseCaching();
/// OUTPUT CACHING
//builder.Services.AddOutputCache(options =>
//{
//    //options.AddBasePolicy(builder =>
//    //{
//    //    builder.Expire(TimeSpan.FromSeconds(30));
//    //    builder.Tag("products");
//    //});
//    options.AddPolicy("SixtySecondsCache", builder =>
//    {
//        builder.Expire(TimeSpan.FromSeconds(60));
//        builder.Tag("products");
//    });
//    options.AddPolicy("NoCache", builder =>
//    {
//        builder.NoCache();
//    });
//});

// Register services from Composition Root
builder.Services.ComposeApplication();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

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

app.UseCors("AllowGetPolicy");

app.UseResponseCaching();  
// app.UseOutputCache();   // must be called after UseCors and after UseRouting if called

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// YARP healthcheck endpoint
app.MapHealthChecks("/api/products/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}).AllowAnonymous();    //.RequireAuthorization("IsAdminOrManager");

// Client healthcheck endpoint
app.MapHealthChecks("/api/products/healthcheck", new HealthCheckOptions
{
    // ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse

    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json; charset=utf-8";

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, WriteIndented = true };

        ////var response = new
        ////{
        ////    status = report.Status.ToString(),
        ////    checks = report.Entries.Select(entry => new
        ////    {
        ////        name = entry.Key,
        ////        status = entry.Value.Status.ToString(),
        ////        description = entry.Value.Description,
        ////        duration = entry.Value.Duration.TotalMilliseconds + "ms"
        ////    }),
        ////    totalDuration = report.TotalDuration.TotalMilliseconds + "ms"
        ////};
        ////await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));

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

        if (report.Status == HealthStatus.Healthy) app.Logger.LogHealthCheckStatus(report.Status.ToString());
        //// DefaultHealthCheckService automatically logs Unhealthy result already, so no need to log error
        //else
        //{
        //    string jsonResult = JsonSerializer.Serialize(dto);
        //    app.Logger.LogError("Health Check Result: {jsonResult}", jsonResult);
        //}

        //// dev purposes only
        // string jsonResult = JsonSerializer.Serialize(dto);
        // app.Logger.LogInformation("Health Check Result: {jsonResult}", jsonResult);

        await context.Response.WriteAsync(JsonSerializer.Serialize(dto, options));
    }

}).AllowAnonymous();

app.Run();
