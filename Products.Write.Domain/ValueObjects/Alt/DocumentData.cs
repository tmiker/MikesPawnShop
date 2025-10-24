
namespace Products.Write.Domain.ValueObjects.Alt
{
    public record DocumentData(
        string Name,
        string Title,
        string SequenceNumber,
        string DocumentUrl)
    {
        public static DocumentData Create(string name, string title, string sequenceNumber, string documentUrl)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));

            if (string.IsNullOrWhiteSpace(sequenceNumber))
                throw new ArgumentException("Sequence Number cannot be empty", nameof(sequenceNumber));

            if (string.IsNullOrWhiteSpace(documentUrl))
                throw new ArgumentException("Document URL cannot be empty", nameof(documentUrl));

            return new DocumentData(name, title, sequenceNumber, documentUrl);
        }

        public string GetDocumentData()
        {
            return $"{Name}, {Title}, {SequenceNumber} {DocumentUrl}";
        }
    }
}
