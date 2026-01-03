namespace Products.Write.Application.CQRS.DevTests
{
    public class AzurePingTestResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public AzurePingTestResult(bool isSuccess, string? errorMessage = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
    }
}
