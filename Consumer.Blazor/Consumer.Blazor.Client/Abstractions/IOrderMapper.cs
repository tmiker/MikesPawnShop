using Consumer.Blazor.Client.DTOs.Carts;
using Consumer.Blazor.Client.DTOs.Orders;

namespace Consumer.Blazor.Client.Abstractions
{
    public interface IOrderMapper
    {
        AddOrderDTO MapCartToAddOrderDTO(ShoppingCartDTO shoppingCartDTO);
        List<AddOrderItemDTO> MapCartItemDTOsToAddOrderItemDTOs(IEnumerable<ShoppingCartItemDTO> cartItemDTOs);
    }
}
