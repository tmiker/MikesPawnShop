using Accounts.API.DTOs;

namespace Accounts.API.Abstractions
{
    public interface IInternalAccountService
    {
        Task<AccountStatusResponseDTO> GetAccountStatus(AccountStatusRequestDTO requestDTO);
    }
}
