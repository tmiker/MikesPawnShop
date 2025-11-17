using Development.Blazor.Client.Paging;

namespace Development.Blazor.DTOs.Tests
{
    public class PagedOutboxRecordResult
    {
        public IEnumerable<OutboxRecordDTO>? OutboxRecords { get; set; }
        public PaginationMetadata? PagingData { get; set; }
    }
}
