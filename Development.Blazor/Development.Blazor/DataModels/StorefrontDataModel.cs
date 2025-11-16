using Development.Blazor.Abstractions;
using Development.Blazor.Client.Utility;
using Development.Blazor.DTOs.Carts;
using Development.Blazor.DTOs.Read;
using Development.Blazor.Paging;
using System.Net.Http;
using System.Text.Json;

namespace Development.Blazor.DataModels
{
    public class StorefrontDataModel
    {
        private readonly IProductsReadHttpService _readProductsClient;

        public StorefrontDataModel(IProductsReadHttpService readProductsClient)
        {
            _readProductsClient = readProductsClient;
        }

        

        public List<ProductSummaryDTO>? Products { get; set; }
        public PaginationMetadata? PagingData { get; set; }
        public ProductSummaryDTO? SelectedProduct { get; set; }
        public ShoppingCartDTO? ShoppingCart { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task GetPagedAndFilteredProductSummariesAsync(string? nameFilter, string? categoryFilter, string? sortColumn, int pageNumber = 1, int pageSize = 10)
        {
            var pagedProductSummariesResult = await _readProductsClient.GetPagedAndFilteredProductSummariesAsync(nameFilter, categoryFilter, sortColumn, pageNumber, pageSize);
            if (pagedProductSummariesResult.IsSuccess)
            {
                if (pagedProductSummariesResult.Products is not null && pagedProductSummariesResult.Products.Any())
                {
                    Products = pagedProductSummariesResult.Products.ToList();
                    PagingData = pagedProductSummariesResult.PagingData;
                }
            }
            else ErrorMessage = pagedProductSummariesResult.ErrorMessage;
        }

        public string? NameFilter { get; set; }
        public string? CategoryFilter { get; set; }
        public string? SortColumn { get; set; }
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        public async Task GetPagedAndFilteredProductSummariesAsync()
        {
            var pagedProductSummariesResult = await _readProductsClient.GetPagedAndFilteredProductSummariesAsync(NameFilter, CategoryFilter, SortColumn, PageNumber, PageSize);
            if (pagedProductSummariesResult.IsSuccess)
            {
                if (pagedProductSummariesResult.Products is not null && pagedProductSummariesResult.Products.Any())
                {
                    Products = pagedProductSummariesResult.Products.ToList();
                    PagingData = pagedProductSummariesResult.PagingData;
                }
            }
            else ErrorMessage = pagedProductSummariesResult.ErrorMessage;
        }
    }
}
