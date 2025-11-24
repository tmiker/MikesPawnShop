using Admin.Blazor.Client.DTOs.Carts;
using Admin.Blazor.Client.DTOs.Orders;

namespace Admin.Blazor.Client.Abstractions
{
    public interface IOrderMapper
    {
        AddOrderDTO MapCartToAddOrderDTO(ShoppingCartDTO shoppingCartDTO);
        List<AddOrderItemDTO> MapCartItemDTOsToAddOrderItemDTOs(IEnumerable<ShoppingCartItemDTO> cartItemDTOs);
    }
}
