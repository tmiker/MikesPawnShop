using Development.Blazor.Paging;

namespace Development.Blazor.DTOs.Read
{
    public class PagedProductSummariesDTO
    {
        public IEnumerable<ProductSummaryDTO>? ProductSummaries { get; set; }
        public PaginationMetadata? PagingData { get; set; }
    }
}
