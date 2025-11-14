using Development.Blazor.Client.Abstractions;
using Development.Blazor.Client.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace Development.Blazor.Client.Services
{
    public class ClientCartHttpService : ICartHttpService
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        private readonly HttpClient _localAPIClient;
        private readonly NavigationManager _navigationManager;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        // Note need to add [FromKeyedServices("LocalAPIClientFromWASM")] attribute to resolve the HttpClient (see Program.cs builder.Services.AddKeyedScoped<HttpClient>)
        public ClientCartHttpService(
            [FromKeyedServices("LocalAPIClientFromWASM")] HttpClient localAPIClient,
            NavigationManager navigationManager,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _localAPIClient = localAPIClient;
            _navigationManager = navigationManager;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<(bool IsSuccess, ApiUserInfoDTO? ApiUserInfo, string? ErrorMessage)> CheckCartsOidcTestEndpointAsync(string? token = null)
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "localapi/cartsproxy/oidcTestEndpoint");
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

                var response = await _localAPIClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                ApiUserInfoDTO? info = await response.Content.ReadFromJsonAsync<ApiUserInfoDTO>();

                return (true, info, null);
            }
            else
            {
                string errorMessage = "Error Testing Endpoint";
                return (false, null, errorMessage);
            }
        }

        public async Task<(bool IsSuccess, ApiUserInfoDTO? ApiUserInfo, string? ErrorMessage)> GetCartsApiUserInfoAsync(string? token = null)
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "localapi/cartsproxy/getApiUserInfo");
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

                var response = await _localAPIClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                ApiUserInfoDTO userInfo = await JsonSerializer.DeserializeAsync<ApiUserInfoDTO>(
                    await response.Content.ReadAsStreamAsync(),
                    _jsonSerializerOptions,
                    CancellationToken.None) ?? new ApiUserInfoDTO();

                return (true, userInfo, null);
            }

            _navigationManager.NavigateTo("/login");
            return (false, null, "Invalid Credentials.");
        }
    }
}
