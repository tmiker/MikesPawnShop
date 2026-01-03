using System.ComponentModel.DataAnnotations;

namespace Admin.Blazor.Client.DTOs.Products.Write
{
    public class UpdateStatusDTO
    {
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public string Status { get; set; } = default!;

        public UpdateStatusDTO(Guid productId, string status)
        {
            ProductId = productId;
            Status = status;
        }
    }
}
