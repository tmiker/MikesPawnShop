namespace Products.Write.Application.CQRS.DevTests
{
    public class ThrowExceptionResult
    {
        public bool IsSuccess { get; init; }
        public string? ErrorMessage { get; init; }

        public ThrowExceptionResult(bool isSuccess, string? errorMessage = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
    }
}
