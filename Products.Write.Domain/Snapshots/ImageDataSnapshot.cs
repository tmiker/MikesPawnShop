namespace Products.Write.Domain.Snapshots
{
    public class ImageDataSnapshot
    {
        public string? Name { get; init; }
        public string? Caption { get; init; }
        public int SequenceNumber { get; init; }
        public string? ImageUrl { get; init; }
        public string? ThumbnailUrl { get; init; }

        public ImageDataSnapshot(string? name, string? caption, int sequenceNumber, string? imageUrl, string? thumbnailUrl)
        {
            Name = name;
            Caption = caption;
            SequenceNumber = sequenceNumber;
            ImageUrl = imageUrl;
            ThumbnailUrl = thumbnailUrl;
        }
    }
}
