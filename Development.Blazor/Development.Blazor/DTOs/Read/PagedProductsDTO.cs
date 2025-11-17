using Development.Blazor.Client.Paging;

namespace Development.Blazor.DTOs.Read
{
    public class PagedProductsDTO
    {
        public IEnumerable<ProductDTO>? Products { get; set; }
        public PaginationMetadata? PagingData { get; set; }
    }
}
