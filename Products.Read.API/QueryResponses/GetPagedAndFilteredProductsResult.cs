using Products.Read.API.DTOs;
using Products.Read.API.Paging;

namespace Products.Read.API.QueryResponses
{
    public class GetPagedAndFilteredProductsResult
    {
        public bool IsSuccess { get; set; }
        public IEnumerable<ProductDTO>? Products { get; set; }
        public PaginationMetadata? PaginationMetadata { get; set; }
        public string? ErrorMessage { get; set; }

        public GetPagedAndFilteredProductsResult(
            bool isSuccess,
            IEnumerable<ProductDTO>? products,
            PaginationMetadata? paginationMetadata,
            string? errorMessage)
        {
            IsSuccess = isSuccess;
            Products = products;
            PaginationMetadata = paginationMetadata;
            ErrorMessage = errorMessage;
        }
    }
}
