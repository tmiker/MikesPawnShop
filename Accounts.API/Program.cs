using Accounts.API.Abstractions;
using Accounts.API.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(setup =>
{
    setup.AddPolicy("AllowAnyPolicy", policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.WithExposedHeaders("X-Pagination");
    });
});

// Configure Auth
JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear(); // Note: As configured, Roles are not populated by HttpContext.User.Claims without this
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:5001";   // IDP
        options.Audience = "accountsapi";            // this api, middleware checks value is in token  
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            NameClaimType = "given_name",       // should have the same mapping as in client app
            RoleClaimType = "role",             // should have the same mapping as in our client mvc app
            ValidTypes = new[] { "at+jwt" }     // says the only valid token type is 'at + jwt'
        };

        //// Optional: Keep claim names as in token
        //options.MapInboundClaims = false;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAdmin", policy => policy.RequireClaim("role", "Admin"));                          // (ClaimTypes.Role, "Admin")); does not work
    options.AddPolicy("IsManager", policy => policy.RequireClaim("role", "Manager"));                      // (ClaimTypes.Role, "Manager")); does not work
    options.AddPolicy("IsAdminOrManager", policy => policy.RequireClaim("role", "Admin", "Manager"));      // (ClaimTypes.Role, "Admin", "Manager"));does not work
    options.AddPolicy("MarlowAndWendy", policy => policy.RequireClaim(ClaimTypes.Name, "Wendy Davenport", "Marlow Bean"));
    options.AddPolicy("DomesticDogs", policy => policy.RequireClaim("Genus", "Canis").RequireClaim("Species", "Familiaris"));
});

builder.Services.AddScoped<ITokenDecoder, TokenDecoder>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Orders API");
        options.WithTheme(ScalarTheme.Alternate);
        options.EnableDarkMode();
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAnyPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
