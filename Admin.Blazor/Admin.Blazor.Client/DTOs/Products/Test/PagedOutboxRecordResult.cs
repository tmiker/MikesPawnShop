using Admin.Blazor.Client.Paging;

namespace Admin.Blazor.Client.DTOs.Products.Test
{
    public class PagedOutboxRecordResult
    {
        public IEnumerable<OutboxRecordDTO>? OutboxRecords { get; set; }
        public PaginationMetadata? PagingData { get; set; }
    }
}
