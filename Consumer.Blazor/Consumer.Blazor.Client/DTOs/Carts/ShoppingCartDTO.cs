using System.Text.Json.Serialization;

namespace Consumer.Blazor.Client.DTOs.Carts
{
    public class ShoppingCartDTO
    {
        public string? Id { get; init; }
        public string? ShoppingCartId { get; init; }
        public int CreditLimit { get; init; }
        public List<ShoppingCartItemDTO> Items { get; init; } = new List<ShoppingCartItemDTO>();

        [JsonIgnore]
        public decimal TotalAmount
        {
            get
            {
                decimal total = 0;
                foreach (var item in Items)
                {
                    total += item.Price * (decimal)item.Quantity;
                }
                return total;
            }
        }
    }
}
