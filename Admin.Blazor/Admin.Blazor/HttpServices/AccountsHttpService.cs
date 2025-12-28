using Admin.Blazor.Client.Abstractions;
using Admin.Blazor.Client.DTOs.Accounts;
using Admin.Blazor.Client.DTOs.Health;
using Admin.Blazor.Client.Utility;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Admin.Blazor.HttpServices
{
    public class AccountsHttpService : IAccountsHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AccountsHttpService> _logger;
        private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, WriteIndented = true };

        public AccountsHttpService(IHttpClientFactory httpClientFactory, ILogger<AccountsHttpService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<(bool IsSuccess, HealthCheckResultDTO? HealthCheckResultDTO, string? ErrorMessage)> CheckHealthAsync(string? token = null)
        {
            string uri = $"{StaticData.AccountsHttpClient_AccountsPath}/health";
            // string uri = "https://localhost:7245/health";  // yarp
            // string uri = "https://localhost:7033/health";
            var client = _httpClientFactory.CreateClient(StaticData.AccountsHttpClient_ClientName);
            if (!string.IsNullOrWhiteSpace(token)) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage response = await client.SendAsync(request);

            try
            {
                response.EnsureSuccessStatusCode();
                var resultDTO = await response.Content.ReadFromJsonAsync<HealthCheckResultDTO>(_jsonSerializerOptions);
                if (resultDTO is not null)
                {
                    _logger.LogInformation($"AccountsHttpService CheckHealthAsync() Result: \n{resultDTO}");
                    return (true, resultDTO, null);
                }
                else return (false, null, "Health check result DTO is null.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountsHttpService CheckHealthAsync() Exception: {ex.Message}");
                return (false, null, ex.Message);
            }
            
            //// return rsult as a json string 
            //var result = await response.Content.ReadAsStringAsync();
            //if (response.IsSuccessStatusCode)
            //{
            //    return (true, result, null);
            //}
            //else
            //{
            //    return (false, null, "Error retrieving health check response.");
            //}
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> AccountIsEstablishedAsync(string? token = null)
        {
            string uri = $"{StaticData.AccountsHttpClient_AccountsPath}/accountEstablished";
            var client = _httpClientFactory.CreateClient(StaticData.AccountsHttpClient_ClientName);
            if (!string.IsNullOrWhiteSpace(token)) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }
            else
            {
                return (false, "An account was not found");
            }
        }

        public async Task<(bool IsSuccess, AccountDTO? Account, string? ErrorMessage)> GetAccountAsync(string? token = null)
        {
            string uri = $"{StaticData.AccountsHttpClient_AccountsPath}";
            var client = _httpClientFactory.CreateClient(StaticData.AccountsHttpClient_ClientName);
            if (!string.IsNullOrWhiteSpace(token)) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                AccountDTO? accountDTO = await response.Content.ReadFromJsonAsync<AccountDTO>();
                if (accountDTO is not null)
                {
                    string jsonAccount = JsonSerializer.Serialize(accountDTO);
                    Console.WriteLine($"\n************\nAccountsHttpService GetAccountAsync() result: \n{jsonAccount}\n************\n");
                }
                return (true, accountDTO, null);
            }
            else
            {
                string errorMessage = await GetErrorMessageAsync(response);
                return (false, null, errorMessage);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateAccountAsync(AddAccountDTO addAccountDTO, string? token = null)
        {
            string uri = $"{StaticData.AccountsHttpClient_AccountsPath}";
            var client = _httpClientFactory.CreateClient(StaticData.AccountsHttpClient_ClientName);
            // if (!string.IsNullOrWhiteSpace(token)) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(addAccountDTO), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }
            else
            {
                string errorMessage = await GetErrorMessageAsync(response);
                return (false, errorMessage);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> AddAddressAsync(AddAddressDTO addAddressDTO, string? token = null)
        {
            string uri = $"{StaticData.AccountsHttpClient_AccountsPath}/addAddress";
            var client = _httpClientFactory.CreateClient(StaticData.AccountsHttpClient_ClientName);
            if (!string.IsNullOrWhiteSpace(token)) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(addAddressDTO), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }
            else
            {
                string errorMessage = await GetErrorMessageAsync(response);
                return (false, errorMessage);
            }
        }

        private async Task<string> GetErrorMessageAsync(HttpResponseMessage response)
        {
            string errorMessage = string.Empty;
            if (!string.IsNullOrEmpty(response.StatusCode.ToString())) errorMessage += $"Status Code: {response.StatusCode.ToString()}; ";
            if (!string.IsNullOrEmpty(response.ReasonPhrase)) errorMessage += $"Reason Phrase: {response.ReasonPhrase}; ";
            string responseContent = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(responseContent)) errorMessage += $"Response Content: {responseContent}; ";
            return errorMessage;
        }
    }
}
