using Consumer.Blazor.Client.Paging;

namespace Consumer.Blazor.Client.DTOs.Products
{
    public class PagedProductsDTO
    {
        public IEnumerable<ProductDTO>? Products { get; set; }
        public PaginationMetadata? PagingData { get; set; }
    }
}
