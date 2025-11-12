using Carts.API.DTOs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Carts.API.Domain.Models
{
    public class ShoppingCartItem
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        // public string? Id { get; set; }
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

        public ShoppingCartItem(ShoppingCartItemDTO shoppingCartItemDTO)
        {
            ShoppingCartId = shoppingCartItemDTO.ShoppingCartId;
            LineNumber = shoppingCartItemDTO.LineNumber;
            ProductId = shoppingCartItemDTO.ProductId;
            Category = shoppingCartItemDTO.Category;
            Name = shoppingCartItemDTO.Name;
            Currency = shoppingCartItemDTO.Currency;
            Price = shoppingCartItemDTO.Price;
            UOM = shoppingCartItemDTO.UOM;
            Quantity = shoppingCartItemDTO.Quantity;
            ThumbnailUrl = shoppingCartItemDTO.ThumbnailUrl;
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
