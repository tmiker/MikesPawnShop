using Orders.API.DTOs;

namespace Orders.API.Abstractions
{
    public interface IInternalAccountsHttpService
    {
        Task<(bool IsSuccess, KeyContainerResponseDTO? KeyContainerResponse, string? ErrorMessage)> GetKeyContainerDataForAccountsAsync();
        Task<AccountStatusResponseDTO> GetUserAccountStatusAsync(AccountStatusRequestDTO accountStatusRequestDTO, CancellationToken? cancellationToken = null);
    }
}
