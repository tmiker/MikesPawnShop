namespace Products.Write.Application.CQRS.CommandResults
{
    public class UpdateStatusResult 
    {
        public bool IsSuccess { get; init; }
        public string? ErrorMessage { get; init; }

        public UpdateStatusResult(bool isSuccess, string? errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
    }
}
