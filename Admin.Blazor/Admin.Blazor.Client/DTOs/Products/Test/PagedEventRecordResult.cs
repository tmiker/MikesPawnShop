using Admin.Blazor.Client.Paging;

namespace Admin.Blazor.Client.DTOs.Products.Test
{
    public class PagedEventRecordResult
    {
        public IEnumerable<EventRecordDTO>? EventRecords { get; set; }
        public PaginationMetadata? PagingData { get; set; }
    }
}
