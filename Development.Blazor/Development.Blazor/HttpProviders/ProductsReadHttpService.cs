using Development.Blazor.Abstractions;
using Development.Blazor.DTOs.Read;
using Development.Blazor.DTOs.Tests;
using Development.Blazor.Client.Utility;
using System.Text;
using System.Text.Json;
using Development.Blazor.Paging;
using Development.Blazor.Client.DTOs;

namespace Development.Blazor.HttpProviders
{
    public class ProductsReadHttpService : IProductsReadHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ProductsReadHttpService> _logger;

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public ProductsReadHttpService(IHttpClientFactory httpClientFactory, ILogger<ProductsReadHttpService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<(bool IsSuccess, ApiUserInfoDTO? ApiUserInfo, string? ErrorMessage)> GetProductsReadApiUserInfoAsync(string? token = null)
        {
            string uri = $"{StaticData.ProductsReadHttpClient_DevTestsPath}{StaticData.ProductsReadHttpClient_GetApiUserInfoSubpath}";
            var client = _httpClientFactory.CreateClient(StaticData.ProductsReadHttpClient_ClientName);

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

        public async IAsyncEnumerable<ProductDTO> StreamProductsAsync()
        {
            string uri = $"{StaticData.ProductsReadHttpClient_ProductsPath}/productStream";
            var client = _httpClientFactory.CreateClient(StaticData.ProductsReadHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

            using (HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                var responseStream = await response.Content.ReadAsStreamAsync();

                var products = JsonSerializer.DeserializeAsyncEnumerable<ProductDTO>(responseStream);

                await foreach (var product in products)
                {
                    Console.WriteLine(product?.Name);
                    yield return product!;
                }
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<ProductDTO>? Products, string? ErrorMessage)> GetProductsAsync()
        {
            string uri = $"{StaticData.ProductsReadHttpClient_ProductsPath}";
            var client = _httpClientFactory.CreateClient(StaticData.ProductsReadHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

            using (HttpResponseMessage response = await client.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    IEnumerable<ProductDTO>? products = JsonSerializer.Deserialize<IEnumerable<ProductDTO>>(result, _jsonOptions);
                    Console.WriteLine(products);
                    return (true, products, null);
                }
                string error = await response.Content.ReadAsStringAsync();
                return (false, null, error);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<ProductSummaryDTO>? ProductSummaries, string? ErrorMessage)> GetProductSummariesAsync()
        {
            string uri = $"{StaticData.ProductsReadHttpClient_ProductsPath}/summaries";
            var client = _httpClientFactory.CreateClient(StaticData.ProductsReadHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

            using (HttpResponseMessage response = await client.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"SUCCESS GETTING PRODUCT SUMMARIES.");
                    string result = await response.Content.ReadAsStringAsync();
                    IEnumerable<ProductSummaryDTO>? productSummaries = JsonSerializer.Deserialize<IEnumerable<ProductSummaryDTO>>(result, _jsonOptions);
                    foreach (var summary in productSummaries!)
                    {
                        Console.WriteLine(summary.Name);
                    }
                    return (true, productSummaries, null);
                }
                string error = await response.Content.ReadAsStringAsync();
                Console.WriteLine(error);
                return (false, null, error);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<ProductDTO>? Products, PaginationMetadata? PagingData, string? ErrorMessage)> GetPagedAndFilteredProductsAsync(
            string? filter, string? category, string? sortColumn, int pageNumber = 1, int pageSize = 10)
        {
            string uri = $"{StaticData.ProductsReadHttpClient_ProductsPath}/paged?filter={filter}&category={category}&sortColumn={sortColumn}&pageNumber={pageNumber}&pageSize={pageSize}";
            var client = _httpClientFactory.CreateClient(StaticData.ProductsReadHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

            using (HttpResponseMessage response = await client.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    PagedProductsDTO? pagedProducts = JsonSerializer.Deserialize<PagedProductsDTO>(result, _jsonOptions);
                    Console.WriteLine(pagedProducts);
                    return (true, pagedProducts?.Products, pagedProducts?.PagingData, null);
                }
                string error = await response.Content.ReadAsStringAsync();
                return (false, null, null, error);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<ProductSummaryDTO>? Products, PaginationMetadata? PagingData, string? ErrorMessage)> GetPagedAndFilteredProductSummariesAsync(
            string? filter, string? category, string? sortColumn, int pageNumber = 1, int pageSize = 10)
        {
            string uri = $"{StaticData.ProductsReadHttpClient_ProductsPath}/paged/summaries?filter={filter}&category={category}&sortColumn={sortColumn}&pageNumber={pageNumber}&pageSize={pageSize}";
            var client = _httpClientFactory.CreateClient(StaticData.ProductsReadHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

            using (HttpResponseMessage response = await client.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    PagedProductSummariesDTO? pagedProductSummaries = JsonSerializer.Deserialize<PagedProductSummariesDTO>(result, _jsonOptions);
                    Console.WriteLine(pagedProductSummaries);
                    return (true, pagedProductSummaries?.ProductSummaries, pagedProductSummaries?.PagingData, null);
                }
                string error = await response.Content.ReadAsStringAsync();
                return (false, null, null, error);
            }
        }

        public async Task<(bool IsSuccess, ProductDTO? Product, string? ErrorMessage)> GetProductByIdAsync(int id)
        {
            string uri = $"{StaticData.ProductsReadHttpClient_ProductsPath}/{id}";
            var client = _httpClientFactory.CreateClient(StaticData.ProductsReadHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

            using (HttpResponseMessage response = await client.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    ProductDTO? product = await response.Content.ReadFromJsonAsync<ProductDTO>();
                    //string result = await response.Content.ReadAsStringAsync();
                    //ProductDTO? product = JsonSerializer.Deserialize<ProductDTO>(result, _jsonOptions);
                    //Console.WriteLine(product);
                    return (true, product, null);
                }
                string error = await response.Content.ReadAsStringAsync();
                return (false, null, error);
            }
        }

        public async Task<(bool IsSuccess, ProductSummaryDTO? ProductSummary, string? ErrorMessage)> GetProductSummaryByIdAsync(int id)
        {
            string uri = $"{StaticData.ProductsReadHttpClient_ProductsPath}/summary/{id}";
            var client = _httpClientFactory.CreateClient(StaticData.ProductsReadHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

            using (HttpResponseMessage response = await client.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    ProductSummaryDTO? productSummary = JsonSerializer.Deserialize<ProductSummaryDTO>(result, _jsonOptions);
                    Console.WriteLine(productSummary);
                    return (true, productSummary, null);
                }
                string error = await response.Content.ReadAsStringAsync();
                return (false, null, error);
            }
        }

        // Dev Tests
        public async Task<(bool IsSuccess, string? ErrorMessage)> ThrowExceptionForTestingAsync(ThrowExceptionDTO throwExceptionDTO, CancellationToken cancellationToken)
        {
            string uri = $"{StaticData.ProductsReadHttpClient_DevTestsPath}/throwExceptionForTesting";
            var client = _httpClientFactory.CreateClient(StaticData.ProductsReadHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(throwExceptionDTO), Encoding.UTF8, "application/json");

            using (HttpResponseMessage response = await client.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("The action ThrowExceptionForTestingAsync(ThrowExceptionDTO throwExceptionDTO) returned " +
                        "HttptatusCode success. It should return Problem Details.");
                    return (true, null);
                }
                string error = await response.Content.ReadAsStringAsync();
                return (false, $"Expected: Problem Details. Actual: {error}");
            }
        }

        public async Task<(bool IsSuccess, string? Value, string? ErrorMessage)> GetCloudAmqpSettingsTestingDummyValueAsync(CancellationToken cancellationToken)
        {
            string uri = $"{StaticData.ProductsReadHttpClient_DevTestsPath}/getCloudAmqpSettingsTestingDummyValue";
            var client = _httpClientFactory.CreateClient(StaticData.ProductsReadHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

            using (HttpResponseMessage response = await client.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    string value = await response.Content.ReadAsStringAsync();
                    return (true, value, null);
                }
                string error = await response.Content.ReadAsStringAsync();
                return (false, null, error);
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
