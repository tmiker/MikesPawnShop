namespace Products.Write.Application.CQRS.CommandResults
{
    public class ProcessMultipleEventsResult
    {
        public bool IsSuccess { get; init; }
        public string? ErrorMessage { get; init; }

        public ProcessMultipleEventsResult(bool isSuccess, string? errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
    }
}
