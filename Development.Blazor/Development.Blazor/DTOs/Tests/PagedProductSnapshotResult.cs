using Development.Blazor.Client.Paging;

namespace Development.Blazor.DTOs.Tests
{
    public class PagedProductSnapshotResult
    {
        public IEnumerable<ProductSnapshotDTO>? ProductSnapshots { get; set; }
        public PaginationMetadata? PagingData { get; set; }
    }
}
