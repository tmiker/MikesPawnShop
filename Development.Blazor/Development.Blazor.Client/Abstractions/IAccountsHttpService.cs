using Development.Blazor.Client.DTOs;
using Development.Blazor.Client.DTOs.Accounts;

namespace Development.Blazor.Client.Abstractions
{
    public interface IAccountsHttpService
    {
        Task<(bool IsSuccess, ApiUserInfoDTO? ApiUserInfo, string? ErrorMessage)> GetAccountsApiUserInfoAsync(string? token = null);
        Task<(bool IsSuccess, string? ErrorMessage)> AccountIsEstablished(string? token = null);
        Task<(bool IsSuccess, AccountDTO? Account, string? ErrorMessage)> GetAccountAsync(string? token = null);
        Task<(bool IsSuccess, string? ErrorMessage)> CreateAccountAsync(AddAccountDTO addAccountDTO, string? token = null);
        Task<(bool IsSuccess, string? ErrorMessage)> AddAddressAsync(AddAddressDTO addAddressDTO, string? token = null);
    }
}
