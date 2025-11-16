using Carts.API.DTOs;

namespace Carts.API.Domain.Models
{
    public class ShoppingCartItem
    {
        public string? ShoppingCartId { get; private set; }
        public int LineNumber { get; private set; }
        public string? ProductId { get; private set; }
        public string? Category { get; private set; }
        public string? Name { get; private set; }
        public string? Currency { get; private set; }
        public decimal Price { get; private set; }
        public string? UOM { get; private set; }
        public double Quantity { get; private set; }
        public string? ThumbnailUrl { get; private set; }

        private ShoppingCartItem() { }

        public ShoppingCartItem(AddShoppingCartItemDTO addShoppingCartItemDTO, string shoppingCartId)
        {
            ShoppingCartId = shoppingCartId;
            LineNumber = addShoppingCartItemDTO.LineNumber;
            ProductId = addShoppingCartItemDTO.ProductId;
            Category = addShoppingCartItemDTO.Category;
            Name = addShoppingCartItemDTO.Name;
            Currency = addShoppingCartItemDTO.Currency;
            Price = addShoppingCartItemDTO.Price;
            UOM = addShoppingCartItemDTO.UOM;
            Quantity = addShoppingCartItemDTO.Quantity;
            ThumbnailUrl = addShoppingCartItemDTO.ThumbnailUrl;
        }

        public void UpdateItemQuantity(double quantity)
        {
            Quantity += quantity;
        }

        public ShoppingCartItemDTO ToShoppingCartItemDTO()
        {
            return new ShoppingCartItemDTO
            {
                ShoppingCartId = ShoppingCartId,
                LineNumber = LineNumber,
                ProductId = ProductId,
                Category = Category,
                Name = Name,
                Currency = Currency,
                Price = Price,
                UOM = UOM,
                Quantity = Quantity,
                ThumbnailUrl = ThumbnailUrl
            };
        }
    }
}
