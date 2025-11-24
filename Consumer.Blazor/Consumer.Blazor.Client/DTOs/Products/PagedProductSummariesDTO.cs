using Consumer.Blazor.Client.Paging;

namespace Consumer.Blazor.Client.DTOs.Products
{
    public class PagedProductSummariesDTO
    {
        public IEnumerable<ProductSummaryDTO>? ProductSummaries { get; set; }
        public PaginationMetadata? PagingData { get; set; }
    }
}
