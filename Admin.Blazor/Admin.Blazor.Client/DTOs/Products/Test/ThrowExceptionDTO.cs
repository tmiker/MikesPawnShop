namespace Admin.Blazor.Client.DTOs.Products.Test
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
