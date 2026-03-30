using Accounts.API.Abstractions;
using Accounts.API.Domain.Models;
using Accounts.API.DTOs;
using MongoDB.Driver;

namespace Accounts.API.Services
{
    public class InternalAccountService : IInternalAccountService
    {
        private readonly IMongoCollection<Account> _accounts;
        private readonly IAccountDataMapper _mapper;
        private readonly IRsaAsymmetricKeyContainerManager _rsaKeyContainerManager;
        private readonly IRsaAsymmetricEncryptionManager _rsaEncryptor;
        private readonly ILogger<InternalAccountService> _logger;

        private const int _baseCreditLimit = 5000;

        public InternalAccountService(
            IMongoSettings mongoSettings, 
            IAccountDataMapper mapper, 
            IRsaAsymmetricKeyContainerManager rsaKeyContainerManager,
            IRsaAsymmetricEncryptionManager rsaEncryptor,
            ILogger<InternalAccountService> logger)
        {
            var client = new MongoClient(mongoSettings.MongoLocalConnection);
            var database = client.GetDatabase(mongoSettings.Database);
            _accounts = database.GetCollection<Account>(mongoSettings.AccountCollection);
            _mapper = mapper;
            _rsaKeyContainerManager = rsaKeyContainerManager;
            _rsaEncryptor = rsaEncryptor;

            _logger = logger;
        }

        public async Task<AccountStatusResponseDTO> GetAccountStatus(AccountStatusRequestDTO requestDTO)
        {
            AccountStatusResponseDTO accountStatusResponse = new AccountStatusResponseDTO();

            if (string.IsNullOrWhiteSpace(requestDTO.EncryptedOwnerId) || string.IsNullOrWhiteSpace(requestDTO.KeyContainerName))
            {
                accountStatusResponse.IsSuccess = false;
                accountStatusResponse.Errors.Add("Missing Required Request Data.");
                return accountStatusResponse;
            }

            // GET KEYS FOR DECRYPTION OF ENCRYPTED OWNERID 
            string publicAndPrivateKeys = _rsaKeyContainerManager.GetPublicAndPrivateKeyForContainerWithName(requestDTO.KeyContainerName);
            _logger.LogInformation("*** {this}: Public and Private Keys retrieved using container name: {keys} ***", this.GetType().Name, publicAndPrivateKeys);   // *** DEV ONLY REMOVE *** //
                                                                                                                                                                   // DECRYPT ENCRYPTED OWNERID 
            string decryptedOwnerId = _rsaEncryptor.DecryptUsingRsaXmlString(requestDTO.EncryptedOwnerId, publicAndPrivateKeys);
            _logger.LogInformation("*** {this}: Decrypted OwnerId using RSA decryption keys. Decrypted OwnerId: {did} ***", this.GetType().Name, decryptedOwnerId); // *** DEV ONLY REMOVE *** //

            _rsaKeyContainerManager.DeleteKeyFromContainer(requestDTO.KeyContainerName);

            if (decryptedOwnerId == null)
            {
                accountStatusResponse.IsSuccess = false;
                accountStatusResponse.Errors.Add("Unable to validate credentials.");
                return accountStatusResponse;
            }
            else
            {
                Account? account = await _accounts.Find(a => a.OwnerId == decryptedOwnerId).FirstOrDefaultAsync();

                accountStatusResponse.Status = account != null ? account.AccountStatus : null;
                if (account is null) accountStatusResponse.Errors.Add("Account not found.");
                Address? billingAddress = account?.Addresses.First(a => a.IsPrimaryShipping == true);
                Address? shippingAddress = account?.Addresses.First(a => a.IsPrimaryShipping == true);
                if (billingAddress is null) accountStatusResponse.Errors.Add("Billing address not found.");
                else accountStatusResponse.BillingAddress = _mapper.MapAddressToDTO(billingAddress);
                if (shippingAddress is null) accountStatusResponse.Errors.Add("Shipping address not found.");
                else accountStatusResponse.ShippingAddress = _mapper.MapAddressToDTO(shippingAddress);

                if (accountStatusResponse.Errors.Count > 0) accountStatusResponse.IsSuccess = false;
                else accountStatusResponse.IsSuccess = true;
                return accountStatusResponse;
            }

        }
    }
}
