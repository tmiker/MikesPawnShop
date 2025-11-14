using Development.Blazor.Client.DTOs;

namespace Development.Blazor.Client.Abstractions
{
    public interface ICartHttpService
    {
        Task<(bool IsSuccess, ApiUserInfoDTO? ApiUserInfo, string? ErrorMessage)> CheckCartsOidcTestEndpointAsync(string? token = null);
        Task<(bool IsSuccess, ApiUserInfoDTO? ApiUserInfo, string? ErrorMessage)> GetCartsApiUserInfoAsync(string? token = null);
    }
}
