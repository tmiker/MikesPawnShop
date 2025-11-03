using Development.Blazor.Paging;

namespace Development.Blazor.DTOs.Tests
{
    public class PagedEventRecordResult
    {
        public IEnumerable<EventRecordDTO>? EventRecords { get; set; }
        public PaginationMetadata? PagingData { get; set; }
    }
}
