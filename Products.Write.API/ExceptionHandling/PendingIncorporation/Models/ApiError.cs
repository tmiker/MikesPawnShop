namespace Products.Write.API.ExceptionHandling.PendingIncorporation.Models
{
    public class ApiError
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Field { get; set; }
        public object? Value { get; set; }
    }
}
