using Products.Read.API.Paging;

namespace Products.Read.API.DTOs
{
    public class PagedProductSummariesDTO
    {
        public IEnumerable<ProductSummaryDTO>? ProductSummaries { get; set; }
        public PaginationMetadata? PagingData { get; set; }

        // add fetch time to monitor whether a response is from cache or not
        public DateTime FetchTime { get; set; } = default;
    }
}
