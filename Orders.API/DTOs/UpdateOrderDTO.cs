namespace Orders.API.DTOs
{
    public class UpdateOrderDTO
    {
        public string? Id { get; set; }
        public string? OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();
        public AddressDTO? ShippingAddress { get; set; }
        public AddressDTO? BillingAddress { get; set; }
    }
}
