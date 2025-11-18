namespace Development.Blazor.Client.DTOs.Orders
{
    public class AddOrderDTO
    {
        // public string? OrderId { get; set; }
        public List<AddOrderItemDTO> Items { get; set; } = new List<AddOrderItemDTO>();
        public AddressDTO? ShippingAddress { get; set; }
        public AddressDTO? BillingAddress { get; set; }
    }
}
