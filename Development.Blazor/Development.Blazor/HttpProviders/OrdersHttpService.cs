using Development.Blazor.Client.Abstractions;
using Development.Blazor.Client.DTOs;
using Development.Blazor.Client.Utility;
using System.Diagnostics;

namespace Development.Blazor.HttpProviders
{
    public class OrdersHttpService : IOrdersHttpService
    {
        private IHttpClientFactory _httpClientFactory;

        public OrdersHttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<(bool IsSuccess, ApiUserInfoDTO? ApiUserInfo, string? ErrorMessage)> GetOrdersApiUserInfoAsync(string? token = null)
        {
            string uri = $"{StaticData.OrdersHttpClient_DevTestsPath}{StaticData.OrdersHttpClient_GetApiUserInfoSubpath}";
            Debug.WriteLine($"GET API USER INFO URI: {uri}");
            var client = _httpClientFactory.CreateClient(StaticData.OrdersHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
            Debug.WriteLine($"GET API USER INFO REQUEST URI: {request.RequestUri}");
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                ApiUserInfoDTO? apiUserInfoDTO = await response.Content.ReadFromJsonAsync<ApiUserInfoDTO>();
                return (true, apiUserInfoDTO, null);
            }
            else
            {
                string errorMessage = await GetErrorMessageAsync(response);
                return (false, new ApiUserInfoDTO() { ErrorMessage = errorMessage }, errorMessage);
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
