using Admin.Blazor.Client.Paging;

namespace Admin.Blazor.Client.DTOs.Products
{
    public class PagedProductsDTO
    {
        public IEnumerable<ProductDTO>? Products { get; set; }
        public PaginationMetadata? PagingData { get; set; }
    }
}
