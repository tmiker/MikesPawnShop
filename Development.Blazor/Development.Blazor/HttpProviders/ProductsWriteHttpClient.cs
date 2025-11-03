using Development.Blazor.Abstractions;
using Development.Blazor.DTOs.Tests;
using Development.Blazor.DTOs.Write;
using Development.Blazor.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Development.Blazor.HttpProviders
{
    public class ProductsWriteHttpClient : IProductsWriteHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ProductsWriteHttpClient> _logger;

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public ProductsWriteHttpClient(IHttpClientFactory httpClientFactory, ILogger<ProductsWriteHttpClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<(bool IsSuccess, Guid? AggregateId, string? ErrorMessage)> AddProductAsync(AddProductDTO addProductDTO, CancellationToken cancellationToken)
        {
            string uri = $"{StaticDetails.ProductsWriteHttpClient_ProductsPath}";
            var client = _httpClientFactory.CreateClient(StaticDetails.ProductsWriteHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
            // Generate a new Correlation ID and add to headers
            string correlationId = Guid.NewGuid().ToString();
            request.Headers.Add("X-Correlation-ID", correlationId);
            request.Content = new StringContent(JsonSerializer.Serialize(addProductDTO), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string aggregateId = await response.Content.ReadAsStringAsync();
                return (true, Guid.Parse(aggregateId), null);
            }
            string error = await response.Content.ReadAsStringAsync();
            return (false, null, error);
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateStatusAsync(UpdateStatusDTO updateStatusDTO, CancellationToken cancellationToken)
        {
            string uri = $"{StaticDetails.ProductsWriteHttpClient_ProductsPath}/status";
            var client = _httpClientFactory.CreateClient(StaticDetails.ProductsWriteHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
            // Generate a new Correlation ID and add to headers
            string correlationId = Guid.NewGuid().ToString();
            request.Headers.Add("X-Correlation-ID", correlationId);
            request.Content = new StringContent(JsonSerializer.Serialize(updateStatusDTO), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);
            string error = await response.Content.ReadAsStringAsync();
            return (false, error);
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> AddImageAsync(AddImageDTO addImageDTO, CancellationToken cancellationToken)
        {
            string uri = $"{StaticDetails.ProductsWriteHttpClient_ProductsPath}/image";
            var client = _httpClientFactory.CreateClient(StaticDetails.ProductsWriteHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
            // Generate a new Correlation ID and add to headers
            string correlationId = Guid.NewGuid().ToString();
            request.Headers.Add("X-Correlation-ID", correlationId);
            request.Content = new StringContent(JsonSerializer.Serialize(addImageDTO), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);
            string error = await response.Content.ReadAsStringAsync();
            return (false, error);
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> AddDocumentAsync(AddDocumentDTO addDocumentDTO, CancellationToken cancellationToken)
        {
            string uri = $"{StaticDetails.ProductsWriteHttpClient_ProductsPath}/document";
            var client = _httpClientFactory.CreateClient(StaticDetails.ProductsWriteHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
            // Generate a new Correlation ID and add to headers
            string correlationId = Guid.NewGuid().ToString();
            request.Headers.Add("X-Correlation-ID", correlationId);
            request.Content = new StringContent(JsonSerializer.Serialize(addDocumentDTO), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);
            string error = await response.Content.ReadAsStringAsync();
            return (false, error);
        }

        // Dev Tests
        public async Task<(bool IsSuccess, string? ErrorMessage)> ThrowExceptionForTestingAsync(ThrowExceptionDTO throwExceptionDTO, CancellationToken cancellationToken)
        {
            string uri = $"{StaticDetails.ProductsWriteHttpClient_DevTestsPath}/throwExceptionForTesting";
            var client = _httpClientFactory.CreateClient(StaticDetails.ProductsWriteHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(throwExceptionDTO), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("The action ThrowExceptionForTestingAsync(ThrowExceptionDTO throwExceptionDTO) returned " +
                    "HttptatusCode success. It should return Problem Details.");
                return (true, null);
            }
            string error = await response.Content.ReadAsStringAsync();
            return (false, $"Expected: Problem Details. Actual: {error}");
        }

        public async Task<(bool IsSuccess, string? Value, string? ErrorMessage)> GetCloudAmqpSettingsTestingDummyValueAsync(CancellationToken cancellationToken)
        {
            string uri = $"{StaticDetails.ProductsWriteHttpClient_DevTestsPath}/getCloudAmqpSettingsTestingDummyValue";
            var client = _httpClientFactory.CreateClient(StaticDetails.ProductsWriteHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string value = await response.Content.ReadAsStringAsync();
                return (true, value, null);
            }
            string error = await response.Content.ReadAsStringAsync();
            return (false, null, error);
        }

        
        public async Task<(bool IsSuccess, string? ErrorMessage)> PurgeDataAsync(PurgeDataDTO purgeDataDTO, CancellationToken cancellationToken)
        {
            string uri = $"{StaticDetails.ProductsWriteHttpClient_DevTestsPath}/purgeData";
            var client = _httpClientFactory.CreateClient(StaticDetails.ProductsWriteHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(purgeDataDTO), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);
            string error = await response.Content.ReadAsStringAsync();
            return (false, error);
        }
    }
}
