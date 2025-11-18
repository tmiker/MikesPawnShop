using Development.Blazor.Abstractions;
using Development.Blazor.Client.DTOs.Carts;
using Development.Blazor.Client.DTOs.Orders;

namespace Development.Blazor.Mappers
{
    public class OrderMapper : IOrderMapper
    {
        public AddOrderDTO MapCartToAddOrderDTO(ShoppingCartDTO shoppingCartDTO)
        {
            return new AddOrderDTO
            {
                 // OrderId = shoppingCartDTO.ShoppingCartId,    // This is set by the Orders.API microservice
                Items = shoppingCartDTO.Items.Select(item => new AddOrderItemDTO
                {
                    ProductId = item.ProductId,
                    AggregateId = item.AggregateId,
                    Category = item.Category,
                    Name = item.Name,
                    Currency = item.Currency,
                    Price = item.Price,
                    UOM = item.UOM,
                    Quantity = item.Quantity,
                }).ToList(),
                // Assuming ShippingAddress and BillingAddress are set elsewhere or are null
                ShippingAddress = null,
                BillingAddress = null
            };
        }
    }
}
