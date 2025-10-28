namespace Products.Write.Application.CQRS.CommandResults
{
    public class AddDocumentResult
    {
        public bool IsSuccess { get; init; }
        public string? ErrorMessage { get; init; }

        public AddDocumentResult(bool isSuccess, string? errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
    }
}
