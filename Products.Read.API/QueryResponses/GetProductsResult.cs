using Products.Read.API.DTOs;

namespace Products.Read.API.QueryResponses
{
    public class GetProductsResult
    {
        public bool IsSuccess { get; set; }
        public IEnumerable<ProductDTO>? Products { get; set; }
        public string? ErrorMessage { get; set; }

        public GetProductsResult(
            bool isSuccess,
            IEnumerable<ProductDTO>? products,
            string? errorMessage)
        {
            IsSuccess = isSuccess;
            Products = products;
            ErrorMessage = errorMessage;
        }
    }
}
