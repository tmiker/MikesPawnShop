using Development.Blazor.Client.DTOs.Carts;
using Development.Blazor.Client.DTOs.Orders;

namespace Development.Blazor.Abstractions
{
    public interface IOrderMapper
    {
        AddOrderDTO MapCartToAddOrderDTO(ShoppingCartDTO shoppingCartDTO);
        List<AddOrderItemDTO> MapCartItemDTOsToAddOrderItemDTOs(IEnumerable<ShoppingCartItemDTO> cartItemDTOs);
    }
}
