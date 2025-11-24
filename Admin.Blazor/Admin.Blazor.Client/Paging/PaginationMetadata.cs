namespace Admin.Blazor.Client.Paging
{
    public class PaginationMetadata
    {
        public int TotalItemCount { get; set; }
        public int TotalPageCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
    }
}
