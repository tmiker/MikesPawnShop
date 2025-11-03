using Development.Blazor.DTOs.Read;
using Development.Blazor.DTOs.Tests;
using Development.Blazor.Paging;

namespace Development.Blazor.Abstractions
{
    public interface IProductsReadHttpClient
    {
        IAsyncEnumerable<ProductDTO> StreamProductsAsync();
        Task<(bool IsSuccess, IEnumerable<ProductDTO>? Products, string? ErrorMessage)> GetProductsAsync();
        Task<(bool IsSuccess, IEnumerable<ProductSummaryDTO>? ProductSummaries, string? ErrorMessage)> GetProductSummariesAsync();
        Task<(bool IsSuccess, IEnumerable<ProductDTO>? Products, PaginationMetadata? PagingData, string? ErrorMessage)> GetPagedAndFilteredProductsAsync(string? filter, string? category, string? sortColumn, int pageNumber = 1, int pageSize = 10);
        Task<(bool IsSuccess, IEnumerable<ProductSummaryDTO>? Products, PaginationMetadata? PagingData, string? ErrorMessage)> GetPagedAndFilteredProductSummariesAsync(string? filter, string? category, string? sortColumn, int pageNumber = 1, int pageSize = 10);
        Task<(bool IsSuccess, ProductDTO? Product, string? ErrorMessage)> GetProductByIdAsync(int id);
        Task<(bool IsSuccess, ProductSummaryDTO? ProductSummary, string? ErrorMessage)> GetProductSummaryByIdAsync(int id);

        // Dev Tests
        Task<(bool IsSuccess, string? ErrorMessage)> ThrowExceptionForTestingAsync(ThrowExceptionDTO throwExceptionDTO, CancellationToken cancellationToken);
        Task<(bool IsSuccess, string? Value, string? ErrorMessage)> GetCloudAmqpSettingsTestingDummyValueAsync(CancellationToken cancellationToken);
    }
}
