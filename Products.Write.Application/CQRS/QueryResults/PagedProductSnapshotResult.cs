using Products.Write.Application.Paging;
using Products.Write.Domain.Snapshots;

namespace Products.Write.Application.CQRS.QueryResults
{
    public class PagedProductSnapshotResult
    {
        public IEnumerable<ProductSnapshot>? Products { get; set; }
        public PaginationMetadata? PagingData { get; set; }
    }
}
