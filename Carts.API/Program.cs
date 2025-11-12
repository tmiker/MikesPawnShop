using Carts.API.Abstractions;
using Carts.API.Auth;
using Carts.API.Infrastructure.Mongo;
using Carts.API.Middleware;
using Carts.API.Services;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<MongoSettings>(builder.Configuration.GetRequiredSection(nameof(MongoSettings)));
builder.Services.AddSingleton<IMongoSettings>(sp => sp.GetRequiredService<IOptions<MongoSettings>>().Value);

builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ITokenDecoder, TokenDecoder>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<CustomLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Carts API");
        options.WithTheme(ScalarTheme.Solarized);
        options.EnableDarkMode();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
