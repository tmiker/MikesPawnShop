namespace Carts.API.DTOs
{
    public class ShoppingCartItemDTO
    {
        public string? ShoppingCartId { get; init; }
        public int LineNumber { get; init; }
        public string? ProductId { get; init; }
        public string? Category { get; init; }
        public string? Name { get; init; }
        public string? Currency { get; init; }
        public decimal Price { get; init; }
        public string? UOM { get; init; }
        public double Quantity { get; init; }
        public string? ThumbnailUrl { get; init; }
    }
}
