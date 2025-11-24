using Consumer.Blazor.Client.Paging;

namespace Consumer.Blazor.Client.DTOs.Orders
{
    public class PagedOrderResultDTO
    {
        public IEnumerable<OrderDTO>? OrderDTOs { get; set; }
        public PaginationMetadata? PagingData { get; set; }
    }
}
