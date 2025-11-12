using Carts.API.Abstractions;
using Carts.API.Domain.Models;
using Carts.API.DTOs;
using MongoDB.Driver;

namespace Carts.API.Services
{
    public class CartService : ICartService
    {
        private readonly IMongoCollection<ShoppingCart> _carts;
        private readonly int _baseCreditLimit = 5000;

        public CartService(IMongoSettings mongoSettings)
        {
            var mongoClient = new MongoClient(mongoSettings.MongoLocalConnection);
            var database = mongoClient.GetDatabase(mongoSettings.Database);
            _carts = database.GetCollection<ShoppingCart>(mongoSettings.ShoppingCartCollection);
        }

        // clients can fetch cart at login or will be retrieved when first attempt to set cart via service

        public async Task<ShoppingCartDTO> GetCartAsync(string ownerId)
        {
            ShoppingCart? cart = await _carts.Find(c => c.OwnerId == ownerId).FirstOrDefaultAsync();
            if (cart is null)
            {
                cart = new ShoppingCart(ownerId, _baseCreditLimit);
                await _carts.InsertOneAsync(cart);
            }
            ShoppingCartDTO cartDTO = cart.ToShoppingCartDTO();
            return cartDTO;
        }

        public async Task<bool> UpdateCartAsync(ShoppingCartDTO cartDTO, string ownerId)
        {
            ShoppingCart? cart = await _carts.Find(c => c.OwnerId == ownerId).FirstOrDefaultAsync();

            if (cart is null)
            {
                cart = new ShoppingCart(cartDTO, ownerId);
                await _carts.InsertOneAsync(cart);
            }
            else
            {
                List<ShoppingCartItem> items = new List<ShoppingCartItem>();
                cartDTO.Items.ForEach(itemDTO => items.Add(new ShoppingCartItem(itemDTO)));
                cart.UpdateCartItems(items);
                await _carts.ReplaceOneAsync(c => c.OwnerId == ownerId, cart);
            }
            return true;
        }

        public async Task<bool> RemoveCartAsync(string ownerId)
        {
            var result = await _carts.DeleteOneAsync(c => c.OwnerId == ownerId);
            if (result.DeletedCount > 0) return true;
            return false;
        }
    }
}
