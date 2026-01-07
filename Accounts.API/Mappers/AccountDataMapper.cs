using Accounts.API.Abstractions;
using Accounts.API.Domain.Models;
using Accounts.API.DTOs;

namespace Accounts.API.Mappers
{
    public class AccountDataMapper : IAccountDataMapper
    {

        public AccountDTO MapAccountToDTO(Account account)
        {
            if (account == null) return null!;
            AccountDTO accountDTO = new AccountDTO
            {
                Id = account.Id,
                AccountId = account.AccountId,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                AccountStatus = account.AccountStatus,
                CreditLimit = account.CreditLimit,
                Addresses = account.Addresses.Select(a => MapAddressToDTO(a)).ToList()
            };
            return accountDTO;
        }

        public AddressDTO MapAddressToDTO(Address address)
        {
            return new AddressDTO
            {
                IsPrimaryBilling = address.IsPrimaryBilling,
                IsPrimaryShipping = address.IsPrimaryShipping,
                Street1 = address.Street1,
                Street2 = address.Street2,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode
            };
        }

        public Account MapAddAccountDtoToAccount(AddAccountDTO addAccountDTO)
        {
            return new Account()
            {
                FirstName = addAccountDTO.FirstName,
                LastName = addAccountDTO.LastName,
                Email = addAccountDTO.Email,
                PhoneNumber = addAccountDTO.PhoneNumber,
                Addresses = addAccountDTO.Addresses.Select(addressDTO => MapAddressDtoToAddress(addressDTO)).ToList()
            };
        }

        public Address MapAddressDtoToAddress(AddressDTO addressDTO)
        {
            return new Address()
            {
                IsPrimaryBilling = addressDTO.IsPrimaryBilling,
                IsPrimaryShipping = addressDTO.IsPrimaryShipping,
                Street1 = addressDTO.Street1,
                Street2 = addressDTO.Street2,
                City = addressDTO.City,
                State = addressDTO.State,
                PostalCode = addressDTO.PostalCode
            };
        }
    }
}