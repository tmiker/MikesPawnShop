namespace Products.Write.Application.CQRS.DevTests
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
