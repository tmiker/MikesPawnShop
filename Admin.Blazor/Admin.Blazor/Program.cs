using Admin.Blazor;
using Admin.Blazor.Client.Abstractions;
using Admin.Blazor.Client.Services;
using Admin.Blazor.Components;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Configure Persisting Auth State
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingAuthenticationStateProvider>();

//// Duende Access Token Management
//builder.Services.AddDistributedMemoryCache();   // to store tokens
//builder.Services.AddOpenIdConnectAccessTokenManagement();   // decorate http client with handler

//// Configure Auth
///

// Services
builder.Services.AddScoped<IToastrService, ToastrService>();

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
    .AddAdditionalAssemblies(typeof(Admin.Blazor.Client._Imports).Assembly);

app.Run();
