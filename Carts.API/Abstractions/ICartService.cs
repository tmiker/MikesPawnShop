using Carts.API.DTOs;

namespace Carts.API.Abstractions
{
    public interface ICartService
    {
        Task<ShoppingCartDTO> GetCartAsync(string ownerId);
        Task<bool> UpdateCartAsync(ShoppingCartDTO cartDTO, string ownerId);
        Task<bool> RemoveCartAsync(string ownerId);
    }
}
