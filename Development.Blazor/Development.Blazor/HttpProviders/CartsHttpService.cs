using Development.Blazor.Client.Abstractions;
using Development.Blazor.Client.DTOs;
using Development.Blazor.Client.DTOs.Carts;
using Development.Blazor.Client.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Development.Blazor.HttpProviders
{
    public class CartsHttpService : ICartsHttpService
    {
        private IHttpClientFactory _httpClientFactory;

        public CartsHttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<(bool IsSuccess, ApiUserInfoDTO? ApiUserInfo, string? ErrorMessage)> GetCartsApiUserInfoAsync(string? token = null)
        {
            string uri = $"{StaticData.CartsHttpClient_DevTestsPath}{StaticData.CartsHttpClient_GetApiUserInfoSubpath}";
            var client = _httpClientFactory.CreateClient(StaticData.CartsHttpClient_ClientName);
            //if (!string.IsNullOrWhiteSpace(token)) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
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

        public async Task<(bool IsSuccess, string? ErrorMessage)> AddNewCartItemAsync(AddShoppingCartItemDTO addShoppingCartItemDTO, string? token = null)
        {
            string uri = $"{StaticData.CartsHttpClient_CartsPath}/items";
            var client = _httpClientFactory.CreateClient(StaticData.CartsHttpClient_ClientName);
            // if (!string.IsNullOrWhiteSpace(token)) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(addShoppingCartItemDTO), Encoding.UTF8, "application/json");
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

        // [HttpPut("items")]
        // public async Task<IActionResult> UpdateProductQuantity(string productId, int amount)
        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateProductQuantityAsync(string productId, int amount, string? token = null)
        {
            string uri = $"{StaticData.CartsHttpClient_CartsPath}/items?productId={productId}&amount={amount}";
            var client = _httpClientFactory.CreateClient(StaticData.CartsHttpClient_ClientName);
            // if (!string.IsNullOrWhiteSpace(token)) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, uri);
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

        // [HttpDelete("items")]
        // public async Task<IActionResult> RemoveCartItem(string productId)
        public async Task<(bool IsSuccess, string? ErrorMessage)> RemoveCartItemAsync(string productId, string? token = null)
        {
            string uri = $"{StaticData.CartsHttpClient_CartsPath}/items?productId={productId}";
            var client = _httpClientFactory.CreateClient(StaticData.CartsHttpClient_ClientName);
            // if (!string.IsNullOrWhiteSpace(token)) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, uri);
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

        // [HttpGet]
        // public async Task<ActionResult<ShoppingCartDTO?>> GetShoppingCart()
        public async Task<(bool IsSuccess, ShoppingCartDTO? ShoppingCart, string? ErrorMessage)> GetShoppingCartAsync(string? token = null)
        {
            string uri = $"{StaticData.CartsHttpClient_CartsPath}";
            var client = _httpClientFactory.CreateClient(StaticData.CartsHttpClient_ClientName);
            // if (!string.IsNullOrWhiteSpace(token)) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                ShoppingCartDTO? cartDTO = await response.Content.ReadFromJsonAsync<ShoppingCartDTO>();
                return (true, cartDTO, null);
            }
            else
            {
                string errorMessage = await GetErrorMessageAsync(response);
                return (false, null, errorMessage);
            }
        }

        // [HttpDelete]
        //public async Task<IActionResult> RemoveShoppingCart()
        public async Task<(bool IsSuccess, string? ErrorMessage)> RemoveShoppingCartAsync(string? token = null)
        {
            string uri = $"{StaticData.CartsHttpClient_CartsPath}";
            var client = _httpClientFactory.CreateClient(StaticData.CartsHttpClient_ClientName);
            // if (!string.IsNullOrWhiteSpace(token)) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, uri);
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode) return (true, null);
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
