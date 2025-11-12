using Carts.API.DTOs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Carts.API.Domain.Models
{
    public class ShoppingCart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; private set; }
        public string? ShoppingCartId { get; private set; }     // to add items before cart is persisted ???
        public string? OwnerId { get; private set; }
        public int CreditLimit { get; private set; }
        public List<ShoppingCartItem> Items { get; private set; } = new List<ShoppingCartItem>();

        private ShoppingCart() { }

        public ShoppingCart(string ownerId, int creditLimit)
        {
            ShoppingCartId = Guid.NewGuid().ToString();
            OwnerId = ownerId;
            CreditLimit = creditLimit;
            Items = new List<ShoppingCartItem>();
        }

        public void UpdateCartItems(IEnumerable<ShoppingCartItem> items)
        {
            Items.Clear();
            Items.AddRange(items);
        }

        public ShoppingCart(ShoppingCartDTO shoppingCartDTO, string ownerId)
        {
            List<ShoppingCartItem> items = new List<ShoppingCartItem>();
            shoppingCartDTO.Items.ForEach(dto => items.Add(new ShoppingCartItem(dto)));
            Id = shoppingCartDTO.Id;
            ShoppingCartId = shoppingCartDTO.ShoppingCartId;
            OwnerId = ownerId;
            CreditLimit = shoppingCartDTO.CreditLimit;
            Items = items;
        }

        public ShoppingCartDTO ToShoppingCartDTO()
        {
            List<ShoppingCartItemDTO> itemDTOs = new List<ShoppingCartItemDTO>();
            Items.ForEach(i => itemDTOs.Add(i.ToShoppingCartItemDTO()));
            return new ShoppingCartDTO()
            {
                Id = Id,
                ShoppingCartId = ShoppingCartId,
                CreditLimit = CreditLimit,
                Items = itemDTOs
            };
        }
    }
}
