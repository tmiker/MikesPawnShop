using Admin.Blazor.Client.DTOs.Accounts;

namespace Admin.Blazor.Client.Abstractions
{
    public interface IAccountsHttpService
    {
        Task<(bool IsSuccess, string? HealthCheckResult, string? ErrorMessage)> CheckHealthAsync(string? token = null);
        Task<(bool IsSuccess, string? ErrorMessage)> AccountIsEstablishedAsync(string? token = null);
        Task<(bool IsSuccess, AccountDTO? Account, string? ErrorMessage)> GetAccountAsync(string? token = null);
        Task<(bool IsSuccess, string? ErrorMessage)> CreateAccountAsync(AddAccountDTO addAccountDTO, string? token = null);
        Task<(bool IsSuccess, string? ErrorMessage)> AddAddressAsync(AddAddressDTO addAddressDTO, string? token = null);
    }
}
