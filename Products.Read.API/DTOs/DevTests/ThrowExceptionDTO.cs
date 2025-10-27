namespace Products.Read.API.DTOs.DevTests
{
    public class ThrowExceptionDTO
    {
        public string ExceptionType { get; init; } = default!;

        public ThrowExceptionDTO(string exceptionType)
        {
            ExceptionType = exceptionType;
        }
    }
}
