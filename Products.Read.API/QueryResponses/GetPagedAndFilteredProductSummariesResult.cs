using Products.Read.API.DTOs;
using Products.Read.API.Paging;

namespace Products.Read.API.QueryResponses
{
    public class GetPagedAndFilteredProductSummariesResult
    {
        public bool IsSuccess { get; set; }
        public IEnumerable<ProductSummaryDTO>? ProductSummaries { get; set; }
        public PaginationMetadata? PaginationMetadata { get; set; }
        public string? ErrorMessage { get; set; }

        public GetPagedAndFilteredProductSummariesResult(
            bool isSuccess, 
            IEnumerable<ProductSummaryDTO>? productSummaries, 
            PaginationMetadata? paginationMetadata, 
            string? errorMessage)
        {
            IsSuccess = isSuccess;
            ProductSummaries = productSummaries;
            PaginationMetadata = paginationMetadata;
            ErrorMessage = errorMessage;
        }
    }
}
