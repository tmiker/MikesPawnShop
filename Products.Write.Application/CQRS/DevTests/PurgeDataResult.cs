namespace Products.Write.Application.CQRS.DevTests
{
    public class PurgeDataResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public PurgeDataResult(bool isSuccess, string? errorMessage = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
    }
}
