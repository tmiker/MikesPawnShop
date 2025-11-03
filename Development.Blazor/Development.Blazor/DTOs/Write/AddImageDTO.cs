namespace Development.Blazor.DTOs.Write
{
    public class AddImageDTO
    {
        public Guid ProductId { get; init; }
        public string Name { get; init; } = default!;
        public string Caption { get; init; } = default!;
        public int SequenceNumber { get; init; }
        public string ImageUrl { get; init; } = default!;
        public string ThumbnailUrl { get; init; } = default!;

        public AddImageDTO(Guid productId, string name, string caption, int sequenceNumber, string imageUrl, string thumbnailUrl)
        {
            ProductId = productId;
            Name = name;
            Caption = caption;
            SequenceNumber = sequenceNumber;
            ImageUrl = imageUrl;
            ThumbnailUrl = thumbnailUrl;
        }
    }
}
