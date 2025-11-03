using Development.Blazor.Paging;

namespace Development.Blazor.DTOs.Tests
{
    public class PagedSnapshotRecordResult
    {
        public IEnumerable<SnapshotRecordDTO>? SnapshotRecords { get; set; }
        public PaginationMetadata? PagingData { get; set; }
    }
}
