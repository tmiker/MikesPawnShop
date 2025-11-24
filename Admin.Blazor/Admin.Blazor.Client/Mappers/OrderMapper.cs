using Admin.Blazor.Client.Abstractions;
using Admin.Blazor.Client.DTOs.Carts;
using Admin.Blazor.Client.DTOs.Orders;

namespace Admin.Blazor.Client.Mappers
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

        public List<AddOrderItemDTO> MapCartItemDTOsToAddOrderItemDTOs(IEnumerable<ShoppingCartItemDTO> cartItemDTOs)
        {
            List<AddOrderItemDTO> orderItemDTOs = new List<AddOrderItemDTO>();
            if (cartItemDTOs is null) return orderItemDTOs;
            foreach (var item in cartItemDTOs)
            {
                AddOrderItemDTO orderItemDTO = new AddOrderItemDTO()
                {
                    //OrderId = null, // populated by Orders.API
                    //LineNumber = item.LineNumber,
                    ProductId = item.ProductId,
                    Category = item.Category,
                    Name = item.Name,
                    Currency = item.Currency,
                    Price = item.Price,
                    UOM = item.UOM,
                    Quantity = item.Quantity
                };
                orderItemDTOs.Add(orderItemDTO);
            }
            return orderItemDTOs;
        }
    }
}
