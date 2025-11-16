using Carts.API.DTOs;

namespace Carts.API.Abstractions
{
    public interface ICartService
    {
        Task<bool> CreateCartAsync(string ownerId);
        Task<ShoppingCartDTO> GetCartAsync(string ownerId);
        Task<bool> RemoveCartAsync(string ownerId);
        Task<bool> AddNewCartItemAsync(string ownerId, AddShoppingCartItemDTO addShoppingCartItemDTO);
        Task<bool> UpdateCartItemQuantityAsync(string ownerId, string productId, double amount);
        Task<bool> RemoveCartItemAsync(string ownerId, string productId);
    }
}
