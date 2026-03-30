
namespace Orders.API.DTOs
{
    public class AccountStatusResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string? Status { get; set; }
        public AddressDTO? BillingAddress { get; set; }
        public AddressDTO? ShippingAddress { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
