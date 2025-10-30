using Products.Read.API.DTOs;

namespace Products.Read.API.QueryResponses
{
    public class GetProductSummaryByIdResult
    {
        public bool IsSuccess { get; set; }
        public ProductSummaryDTO? ProductSummary { get; set; }
        public string? ErrorMessage { get; set; }

        public GetProductSummaryByIdResult(
            bool isSuccess,
            ProductSummaryDTO? productSummary,
            string? errorMessage)
        {
            IsSuccess = isSuccess;
            ProductSummary = productSummary;
            ErrorMessage = errorMessage;
        }
    }
}
