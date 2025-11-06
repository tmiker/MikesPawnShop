using Development.Blazor.Abstractions;
using Development.Blazor.DTOs.Tests;
using Development.Blazor.DTOs.Write;
using Development.Blazor.Paging;
using Development.Blazor.Utility;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
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

        // Write Products path
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

        // UPDATED METHODS FOR IMAGES AND DOCUMENTS
        public async Task<(bool IsSuccess, string? ErrorMessage)> AddProductImageAsync(AddImageDTO addImageDTO, CancellationToken cancellationToken)
        {
            string uri = $"{StaticDetails.ProductsWriteHttpClient_ProductsPath}/image";
            var client = _httpClientFactory.CreateClient(StaticDetails.ProductsWriteHttpClient_ClientName);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);

            // build form file to submit to api endpoint
            using (var content = new MultipartFormDataContent())
            {
                //no Id for add
                if (!string.IsNullOrWhiteSpace(addImageDTO.ProductId)) content.Add(new StringContent(addImageDTO.ProductId!), nameof(addImageDTO.ProductId));
                if (!string.IsNullOrWhiteSpace(addImageDTO.Name)) content.Add(new StringContent(addImageDTO.Name!), nameof(addImageDTO.Name));
                if (!string.IsNullOrWhiteSpace(addImageDTO.Caption)) content.Add(new StringContent(addImageDTO.Caption!), nameof(addImageDTO.Caption));
                content.Add(new StringContent(addImageDTO.SequenceNumber.ToString()), nameof(addImageDTO.SequenceNumber));
                if (!string.IsNullOrWhiteSpace(addImageDTO.BlobFileName)) content.Add(new StringContent(addImageDTO.BlobFileName!), nameof(addImageDTO.BlobFileName));
                if (addImageDTO.ImageBlob is not null)
                {
                    var image = addImageDTO.ImageBlob;
                    var fileContent = new StreamContent(image.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
                    content.Add(content: fileContent, name: "ImageBlob", fileName: addImageDTO.ImageBlob.Name);

                    request.Content = content;

                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode) return (true, null);
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        return (false, error);
                    }
                }

                else return (false, "No image was provided.");
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> AddProductDocumentAsync(AddDocumentDTO addDocumentDTO, CancellationToken cancellationToken)
        {
            if (addDocumentDTO!.DocumentBlob is not null) Console.WriteLine($"The HTTP CLIENT Document Blob IS NOT null.");
            else Console.WriteLine($"The HTTP CLIENT Document Blob IS null.");

            string uri = $"{StaticDetails.ProductsWriteHttpClient_ProductsPath}/document";
            var client = _httpClientFactory.CreateClient(StaticDetails.ProductsWriteHttpClient_ClientName);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);

            // build form file to submit to api endpoint
            using (var content = new MultipartFormDataContent())
            {
                //no Id for add
                if (!string.IsNullOrWhiteSpace(addDocumentDTO.ProductId)) content.Add(new StringContent(addDocumentDTO.ProductId!), nameof(addDocumentDTO.ProductId));
                if (!string.IsNullOrWhiteSpace(addDocumentDTO.Name)) content.Add(new StringContent(addDocumentDTO.Name!), nameof(addDocumentDTO.Name));
                if (!string.IsNullOrWhiteSpace(addDocumentDTO.Title)) content.Add(new StringContent(addDocumentDTO.Title!), nameof(addDocumentDTO.Title));
                content.Add(new StringContent(addDocumentDTO.SequenceNumber.ToString()), nameof(addDocumentDTO.SequenceNumber));
                if (!string.IsNullOrWhiteSpace(addDocumentDTO.BlobFileName)) content.Add(new StringContent(addDocumentDTO.BlobFileName!), nameof(addDocumentDTO.BlobFileName));
                if (addDocumentDTO.DocumentBlob is not null)
                {
                    IBrowserFile blob = addDocumentDTO.DocumentBlob;
                    var fileContent = new StreamContent(blob.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(blob.ContentType);
                    content.Add(content: fileContent, name: "DocumentBlob", fileName: addDocumentDTO.DocumentBlob.Name);

                    request.Content = content;

                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode) return (true, null);
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        return (false, error);
                    }
                }
                else return (false, "No document was provided.");
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteProductImageAsync(DeleteImageDTO deleteImageDTO)
        {
            string uri = $"{StaticDetails.ProductsWriteHttpClient_ProductsPath}/image";
            var client = _httpClientFactory.CreateClient(StaticDetails.ProductsWriteHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(deleteImageDTO), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                return (false, error);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteProductDocumentAsync(DeleteDocumentDTO deleteDocumentDTO)
        {
            string uri = $"{StaticDetails.ProductsWriteHttpClient_ProductsPath}/document";
            var client = _httpClientFactory.CreateClient(StaticDetails.ProductsWriteHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(deleteDocumentDTO), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                return (false, error);
            }
        }

        // Dev Tests
        // Dev Tests methods
        public async Task<(bool IsSuccess, IEnumerable<ProductSnapshotDTO>? ProductSnapshots, PaginationMetadata? PagingData, string? ErrorMessage)> GetProductSnapshotsAsync(
            string? aggregateId,
            int minVersion = 0,
            int maxVersion = Int32.MaxValue,
            int pageNumber = 1,
            int pageSize = 10)
        {
            string uri = $"{StaticDetails.ProductsWriteHttpClient_DevTestsPath}/productSnapshots?aggregateId={aggregateId}&minVersion={minVersion}&maxVersion={maxVersion}&pageNumber={pageNumber}&pageSize={pageSize}";
            var client = _httpClientFactory.CreateClient(StaticDetails.ProductsWriteHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                PagedProductSnapshotResult? result = await response.Content.ReadFromJsonAsync<PagedProductSnapshotResult>();
                return (true, result?.ProductSnapshots, result?.PagingData, null);

            }
            string error = await response.Content.ReadAsStringAsync();
            return (false, null, null, error);
        }

        public async Task<(bool IsSuccess, IEnumerable<EventRecordDTO>? EventRecords, PaginationMetadata? PagingData, string? ErrorMessage)> GetEventRecordsAsync(
            string? aggregateId,
            string? correlationId = null,
            int minVersion = 0,
            int maxVersion = Int32.MaxValue,
            int pageNumber = 1,
            int pageSize = 10)
        {
            string uri = $"{StaticDetails.ProductsWriteHttpClient_DevTestsPath}/eventRecords?aggregateId={aggregateId}&correlationId={correlationId}&minVersion={minVersion}&maxVersion={maxVersion}&pageNumber={pageNumber}&pageSize={pageSize}";
            _logger.LogInformation("GET EVENT RECORDS URI: {uri}", uri);
            var client = _httpClientFactory.CreateClient(StaticDetails.ProductsWriteHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                PagedEventRecordResult? result = await response.Content.ReadFromJsonAsync<PagedEventRecordResult>();
                return (true, result?.EventRecords, result?.PagingData, null);

            }
            string error = await response.Content.ReadAsStringAsync();
            return (false, null, null, error);
        }

        public async Task<(bool IsSuccess, IEnumerable<OutboxRecordDTO>? OutboxRecords, PaginationMetadata? PagingData, string? ErrorMessage)> GetOutboxRecordsAsync(
            string? aggregateId,
            string? correlationId = null,
            int minVersion = 0,
            int maxVersion = Int32.MaxValue,
            int pageNumber = 1,
            int pageSize = 10)
        {
            string uri = $"{StaticDetails.ProductsWriteHttpClient_DevTestsPath}/outboxRecords?aggregateId={aggregateId}&correlationId={correlationId}&minVersion={minVersion}&maxVersion={maxVersion}&pageNumber={pageNumber}&pageSize={pageSize}";
            var client = _httpClientFactory.CreateClient(StaticDetails.ProductsWriteHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                PagedOutboxRecordResult? result = await response.Content.ReadFromJsonAsync<PagedOutboxRecordResult>();
                return (true, result?.OutboxRecords, result?.PagingData, null);

            }
            string error = await response.Content.ReadAsStringAsync();
            return (false, null, null, error);
        }

        public async Task<(bool IsSuccess, IEnumerable<SnapshotRecordDTO>? SnapshotRecords, PaginationMetadata? PagingData, string? ErrorMessage)> GetSnapshotRecordsAsync(
            string? aggregateId,
            string? correlationId = null,
            int minVersion = 0,
            int maxVersion = Int32.MaxValue,
            int pageNumber = 1,
            int pageSize = 10)
        {
            string uri = $"{StaticDetails.ProductsWriteHttpClient_DevTestsPath}/snapshotRecords?aggregateId={aggregateId}&correlationId={correlationId}&minVersion={minVersion}&maxVersion={maxVersion}&pageNumber={pageNumber}&pageSize={pageSize}";
            var client = _httpClientFactory.CreateClient(StaticDetails.ProductsWriteHttpClient_ClientName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                PagedSnapshotRecordResult? result = await response.Content.ReadFromJsonAsync<PagedSnapshotRecordResult>();
                return (true, result?.SnapshotRecords, result?.PagingData, null);

            }
            string error = await response.Content.ReadAsStringAsync();
            return (false, null, null, error);
        }

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
