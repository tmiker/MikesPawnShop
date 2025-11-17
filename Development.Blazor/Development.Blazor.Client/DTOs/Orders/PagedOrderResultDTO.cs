using Development.Blazor.Client.Paging;

namespace Development.Blazor.Client.DTOs.Orders
{
    public class PagedOrderResultDTO
    {
        public IEnumerable<OrderDTO>? OrderDTOs { get; set; }
        public PaginationMetadata? PagingData { get; set; }
    }
}
