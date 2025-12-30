using Admin.Blazor.Client.Paging;

namespace Admin.Blazor.Client.DTOs.Products
{
    public class PagedProductSummariesDTO
    {
        public IEnumerable<ProductSummaryDTO>? ProductSummaries { get; set; }
        public PaginationMetadata? PagingData { get; set; }

        // map fetch time to monitor whether a response is from cache or not
        public DateTime FetchTime { get; set; }
    }
}
