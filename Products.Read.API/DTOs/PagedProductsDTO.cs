using Products.Read.API.Paging;

namespace Products.Read.API.DTOs
{
    public class PagedProductsDTO
    {
        public IEnumerable<ProductDTO>? Products { get; set; }
        public PaginationMetadata? PagingData { get; set; }
    }
}
