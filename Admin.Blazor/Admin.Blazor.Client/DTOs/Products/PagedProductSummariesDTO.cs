using Admin.Blazor.Client.Paging;

namespace Admin.Blazor.Client.DTOs.Products
{
    public class PagedProductSummariesDTO
    {
        public IEnumerable<ProductSummaryDTO>? ProductSummaries { get; set; }
        public PaginationMetadata? PagingData { get; set; }
    }
}
