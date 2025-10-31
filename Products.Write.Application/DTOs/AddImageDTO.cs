namespace Products.Write.Application.DTOs
{
    public class AddImageDTO
    {
        public Guid ProductId { get; init; }
        public string Name { get; init; } = default!;
        public string Caption { get; init; } = default!;
        public int SequenceNumber { get; init; }
        public string ImageUrl { get; init; } = default!;
        public string ThumbnailUrl { get; init; } = default!;
    }
}
