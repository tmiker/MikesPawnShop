using Admin.Blazor.Client.Paging;

namespace Admin.Blazor.Client.DTOs.Orders
{
    public class PagedOrderResultDTO
    {
        public IEnumerable<OrderDTO>? OrderDTOs { get; set; }
        public PaginationMetadata? PagingData { get; set; }
    }
}
