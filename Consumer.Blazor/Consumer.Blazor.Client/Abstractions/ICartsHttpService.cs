using Consumer.Blazor.Client.DTOs.Carts;

namespace Consumer.Blazor.Client.Abstractions
{
    public interface ICartsHttpService
    {
        Task<(bool IsSuccess, int CartItemQuantity, string? ErrorMessage)> AddNewCartItemAsync(AddShoppingCartItemDTO addShoppingCartItemDTO, string? token = null);
        Task<(bool IsSuccess, string? ErrorMessage)> UpdateProductQuantityAsync(string aggregateId, int amount, string? token = null);
        Task<(bool IsSuccess, string? ErrorMessage)> RemoveCartItemAsync(string aggregateId, string? token = null);
        Task<(bool IsSuccess, ShoppingCartDTO? ShoppingCart, string? ErrorMessage)> GetShoppingCartAsync(string? token = null);
        Task<(bool IsSuccess, string? ErrorMessage)> RemoveShoppingCartAsync(string? token = null);
    }
}
