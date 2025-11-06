namespace Development.Blazor.DTOs.Write
{
    public class AddProductResult
    {
        public bool IsSuccess { get; init; }
        public Guid ProductId { get; init; }
        public string? ErrorMessage { get; init; }

        public AddProductResult(bool isSuccess, Guid productId, string? errorMessage = null)
        {
            IsSuccess = isSuccess;
            ProductId = productId;
            ErrorMessage = errorMessage;
        }
    }
}
