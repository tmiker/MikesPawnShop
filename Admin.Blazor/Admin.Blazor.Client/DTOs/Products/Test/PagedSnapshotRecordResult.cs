using Admin.Blazor.Client.Paging;

namespace Admin.Blazor.Client.DTOs.Products.Test
{
    public class PagedSnapshotRecordResult
    {
        public IEnumerable<SnapshotRecordDTO>? SnapshotRecords { get; set; }
        public PaginationMetadata? PagingData { get; set; }
    }
}
