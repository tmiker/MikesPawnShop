namespace Products.Write.Application.CQRS.CommandResults
{
    public class AddImageResult
    {
        public bool IsSuccess { get; init; }
        public string? ErrorMessage { get; init; }

        public AddImageResult(bool isSuccess, string? errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
    }
}
