namespace Development.Blazor.DTOs.Carts
{
    public class ShoppingCartDTO
    {
        public string? Id { get; init; }
        public string? ShoppingCartId { get; init; }
        public int CreditLimit { get; init; }
        public List<ShoppingCartItemDTO> Items { get; init; } = new List<ShoppingCartItemDTO>();
    }
}
