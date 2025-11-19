namespace Accounts.API.DTOs
{
    public class EditAccountDTO
    {
        public string? AccountId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public List<AddressDTO> Addresses { get; set; } = new List<AddressDTO>();
        public string? PhoneNumber { get; set; }
        public string? AccountStatus { get; set; }
        public int CreditLimit { get; set; }
    }
}
