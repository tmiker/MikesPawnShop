using System.ComponentModel.DataAnnotations;

namespace Admin.Blazor.Client.DTOs.Carts
{
    public class AddShoppingCartItemDTO
    {
        [Required]
        public int ProductId { get; init; }
        [Required]
        public string? AggregateId { get; init; }
        public string? Category { get; init; }
        [Required]
        public string? Name { get; init; }
        [Required]
        public string? Currency { get; init; }
        public decimal Price { get; init; }
        [Required]
        public string? UOM { get; init; }
        public double Quantity { get; init; }
        public string? ThumbnailUrl { get; init; }
    }
}
