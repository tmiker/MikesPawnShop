using Products.Read.API.DTOs;

namespace Products.Read.API.QueryResponses
{
    public class GetProductByIdResult
    {
        public bool IsSuccess { get; set; }
        public ProductDTO? Product { get; set; }
        public string? ErrorMessage { get; set; }

        public GetProductByIdResult(
            bool isSuccess,
            ProductDTO? product,
            string? errorMessage)
        {
            IsSuccess = isSuccess;
            Product = product;
            ErrorMessage = errorMessage;
        }
    }
}
