using Products.Read.API.Paging;

namespace Products.Read.API.DTOs
{
    public class PagedProductSummariesDTO
    {
        public IEnumerable<ProductSummaryDTO>? ProductSummaries { get; set; }
        public PaginationMetadata? PagingData { get; set; }
    }
}
