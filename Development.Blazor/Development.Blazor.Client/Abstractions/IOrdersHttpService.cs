using Development.Blazor.Client.DTOs;

namespace Development.Blazor.Client.Abstractions
{
    public interface IOrdersHttpService
    {
        Task<(bool IsSuccess, ApiUserInfoDTO? ApiUserInfo, string? ErrorMessage)> GetOrdersApiUserInfoAsync(string? token = null);
    }
}
