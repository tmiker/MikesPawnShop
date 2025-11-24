using Admin.Blazor.Client.Paging;

namespace Admin.Blazor.Client.DTOs.Products.Test
{
    public class PagedProductSnapshotResult
    {
        public IEnumerable<ProductSnapshotDTO>? ProductSnapshots { get; set; }
        public PaginationMetadata? PagingData { get; set; }
    }
}
