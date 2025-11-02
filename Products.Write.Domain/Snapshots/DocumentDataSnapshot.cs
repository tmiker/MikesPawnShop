namespace Products.Write.Domain.Snapshots
{
    public class DocumentDataSnapshot
    {
        public string? Name { get; init; }           
        public string? Title { get; init; }
        public int SequenceNumber { get; init; }
        public string? DocumentUrl { get; init; }

        public DocumentDataSnapshot(string? name, string? title, int sequenceNumber, string? documentUrl)
        {
            Name = name;
            Title = title;
            SequenceNumber = sequenceNumber;
            DocumentUrl = documentUrl;
        }
    }
}
