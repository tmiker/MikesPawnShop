using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Web;
using Development.Blazor.Client;

namespace Development.Blazor
{
    // This is a server-side AuthenticationStateProvider that uses PersistentComponentState to flow the
    // authentication state to the client which is then fixed for the lifetime of the WebAssembly application.

    // Requires adding UserInfo.cs model

    internal sealed class PersistingAuthenticationStateProvider : AuthenticationStateProvider, IHostEnvironmentAuthenticationStateProvider, IDisposable
    {
        private readonly PersistentComponentState _persistentComponentState;
        private readonly PersistingComponentStateSubscription _subscription;
        private Task<AuthenticationState>? _authenticationStateTask;

        public PersistingAuthenticationStateProvider(PersistentComponentState state)
        {
            _persistentComponentState = state;
            _subscription = state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync() => _authenticationStateTask ??
                throw new InvalidOperationException($"Do not call {nameof(GetAuthenticationStateAsync)} outside of the DI scope for a Razor component. Typically, this means you can call it only within a Razor component or inside another DI service that is resolved for a Razor component.");

        public void SetAuthenticationState(Task<AuthenticationState> task)
        {
            _authenticationStateTask = task;
        }

        private async Task OnPersistingAsync()
        {
            var authenticationState = await GetAuthenticationStateAsync();
            var principal = authenticationState.User;

            if (principal.Identity?.IsAuthenticated == true)
            {
                var userId = GetRequiredClaim(principal, "sub");
                var name = GetRequiredClaim(principal, "name");                                             // JwtClaimTypes.Name
                var email = principal.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
                if (string.IsNullOrEmpty(email)) email = "No email claim found";
                // var email = GetRequiredClaim(principal, JwtClaimTypes.Email);   // "email");             // JwtClaimTypes.Email
                // var country = GetRequiredClaim(principal, "ctry");

                if (userId != null && name != null)
                {
                    _persistentComponentState.PersistAsJson(nameof(UserInfo), new UserInfo
                    {
                        UserId = userId,
                        Name = name,
                        Email = email
                        // Country = country
                    });
                }
            }
        }

        private string GetRequiredClaim(ClaimsPrincipal principal, string claimType) =>
           principal.FindFirst(claimType)?.Value ?? throw new InvalidOperationException($"Could not find required '{claimType}' claim.");


        public void Dispose()
        {
            _subscription.Dispose();
        }
    }
}
