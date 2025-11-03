namespace Development.Blazor.DTOs.Write
{
    public class AddDocumentDTO
    {
        public Guid ProductId { get; init; }
        public string Name { get; init; } = default!;
        public string Title { get; init; } = default!;
        public int SequenceNumber { get; init; }
        public string DocumentUrl { get; init; } = default!;

        public AddDocumentDTO(Guid productId, string name, string title, int sequenceNumber, string documentUrl)
        {
            ProductId = productId;
            Name = name;
            Title = title;
            SequenceNumber = sequenceNumber;
            DocumentUrl = documentUrl;
        }
    }
}
