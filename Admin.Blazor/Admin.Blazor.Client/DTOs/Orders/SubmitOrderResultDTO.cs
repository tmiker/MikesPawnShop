namespace Admin.Blazor.Client.DTOs.Orders
{
    public class SubmitOrderResultDTO
    {
        public bool IsSuccess { get; init; }
        public string? OrderId { get; init; }
        public string? ErrorMessage { get; init; }

        public SubmitOrderResultDTO(bool isSuccess, string? orderId, string? errorMessage)
        {
            IsSuccess = isSuccess;
            OrderId = orderId;
            ErrorMessage = errorMessage;
        }
    }
}
