using Development.Blazor.Abstractions;
using Development.Blazor.Client.Pages;
using Development.Blazor.Components;
using Development.Blazor.HttpProviders;
using Development.Blazor.Utility;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Http Clients
builder.Services.AddHttpClient(name: StaticDetails.ProductsReadHttpClient_ClientName, configureClient: config =>
{
    config.BaseAddress = new Uri(StaticDetails.ProductsReadHttpClient_BaseURL);
    config.DefaultRequestHeaders.Clear();
    config.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});
builder.Services.AddSingleton<IProductsReadHttpClient, ProductsReadHttpClient>();

builder.Services.AddHttpClient(name: StaticDetails.ProductsWriteHttpClient_ClientName, configureClient: config =>
{
    config.BaseAddress = new Uri(StaticDetails.ProductsWriteHttpClient_BaseURL);
    config.DefaultRequestHeaders.Clear();
    config.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});
builder.Services.AddSingleton<IProductsWriteHttpClient, ProductsWriteHttpClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Development.Blazor.Client._Imports).Assembly);

app.Run();
