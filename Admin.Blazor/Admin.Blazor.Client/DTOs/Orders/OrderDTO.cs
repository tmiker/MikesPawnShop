using Admin.Blazor.Client.DTOs.Accounts;

namespace Admin.Blazor.Client.DTOs.Orders
{
    public class OrderDTO
    {
        public string? Id { get; set; }
        public string? OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();
        public string? Status { get; set; }
        public int Version { get; set; }
        public AddressDTO? ShippingAddress { get; set; }
        public AddressDTO? BillingAddress { get; set; }
    }
}
