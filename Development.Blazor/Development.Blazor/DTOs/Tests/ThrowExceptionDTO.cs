namespace Development.Blazor.DTOs.Tests
{
    public class ThrowExceptionDTO
    {
        public string ExceptionType { get; set; } = default!;

        public ThrowExceptionDTO(string exceptionType)
        {
            ExceptionType = exceptionType;
        }
    }
}
