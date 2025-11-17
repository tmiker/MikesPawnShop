namespace Orders.API.Paging
{
    public class PaginationMetadata
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalItemCount { get; set; }
        public int TotalPageCount { get; set; }

        public PaginationMetadata(int totalItems, int pageSize, int currentPage)
        {
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalItemCount = totalItems;
            TotalPageCount = (int)Math.Ceiling(totalItems / (double)pageSize);
        }
    }
}
