using Development.Blazor.Client.DTOs;

namespace Development.Blazor.Client.Abstractions
{
    public interface ICartsHttpService
    {
        Task<(bool IsSuccess, ApiUserInfoDTO? ApiUserInfo, string? ErrorMessage)> GetCartsApiUserInfoAsync(string? token = null);
    }
}
