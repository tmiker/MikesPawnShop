namespace Accounts.API.DTOs
{
    public class AccountStatusRequestDTO
    {
        // OWNER ID ENCRYPTED
        public string? KeyContainerName { get; set; }
        public string? EncryptedOwnerId { get; set; }
    }
}
