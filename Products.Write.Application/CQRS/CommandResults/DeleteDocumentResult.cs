namespace Products.Write.Application.CQRS.CommandResults
{
    public class DeleteDocumentResult
    {
        public bool IsSuccess { get; init; }
        public string? ErrorMessage { get; init; }

        public DeleteDocumentResult(bool isSuccess, string? errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
    }
}
