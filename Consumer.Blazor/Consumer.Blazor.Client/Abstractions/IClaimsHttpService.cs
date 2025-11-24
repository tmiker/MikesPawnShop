using Consumer.Blazor.Client.DTOs.Claims;

namespace Consumer.Blazor.Client.Abstractions
{
    public interface IClaimsHttpService
    {
        Task<(bool IsSuccess, ApiUserInfoDTO? ApiUserInfo, string? ErrorMessage)> GetAccountsApiUserInfoAsync(string? token = null);
        Task<(bool IsSuccess, ApiUserInfoDTO? ApiUserInfo, string? ErrorMessage)> GetCartsApiUserInfoAsync(string? token = null);
        Task<(bool IsSuccess, ApiUserInfoDTO? ApiUserInfo, string? ErrorMessage)> GetOrdersApiUserInfoAsync(string? token = null);
        Task<(bool IsSuccess, ApiUserInfoDTO? ApiUserInfo, string? ErrorMessage)> GetProductsReadApiUserInfoAsync(string? token = null);
    }
}
