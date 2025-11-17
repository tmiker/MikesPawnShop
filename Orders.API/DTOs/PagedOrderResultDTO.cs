using Orders.API.Paging;

namespace Orders.API.DTOs
{
    public class PagedOrderResultDTO
    {
        public IEnumerable<OrderDTO>? OrderDTOs { get; set; }
        public PaginationMetadata? PagingData { get; set; }
    }
}
