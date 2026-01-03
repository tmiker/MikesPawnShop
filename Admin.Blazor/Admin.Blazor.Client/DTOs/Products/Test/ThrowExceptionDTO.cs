using System.ComponentModel.DataAnnotations;

namespace Admin.Blazor.Client.DTOs.Products.Test
{
    public class ThrowExceptionDTO
    {
        [Required]
        public string ExceptionType { get; set; } = default!;

        public ThrowExceptionDTO(string exceptionType)
        {
            ExceptionType = exceptionType;
        }
    }
}
