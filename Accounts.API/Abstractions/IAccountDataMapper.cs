using Accounts.API.Domain.Models;
using Accounts.API.DTOs;

namespace Accounts.API.Abstractions
{
    public interface IAccountDataMapper
    {
        AccountDTO MapAccountToDTO(Account account);
        AddressDTO MapAddressToDTO(Address address);
        Account MapAddAccountDtoToAccount(AddAccountDTO addAccountDTO);
        Address MapAddressDtoToAddress(AddressDTO addressDTO);
    }
}
