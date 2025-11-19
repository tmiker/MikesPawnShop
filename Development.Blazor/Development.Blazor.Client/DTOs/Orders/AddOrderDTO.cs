using Development.Blazor.Client.DTOs.Shared;

namespace Development.Blazor.Client.DTOs.Orders
{
    public class AddOrderDTO
    {
        // public string? OrderId { get; set; }
        public List<AddOrderItemDTO> Items { get; set; } = new List<AddOrderItemDTO>();
        public AddressDTO? ShippingAddress { get; set; }
        public AddressDTO? BillingAddress { get; set; }

        public decimal OrderTotalPrice 
        { 
            get
            {
                decimal total = 0;
                foreach (var item in Items)
                {
                    total += item.Price * (decimal)item.Quantity;
                }
                return total;
            }
        }
    }
}
