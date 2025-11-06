namespace Products.Write.Application.CQRS.CommandResults
{
    public class DeleteImageResult
    {
        public bool IsSuccess { get; init; }
        public string? ErrorMessage { get; init; }

        public DeleteImageResult(bool isSuccess, string? errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
    }
}
