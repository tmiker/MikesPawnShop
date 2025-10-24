using Products.Write.Domain.Base;

namespace Products.Write.Domain.ValueObjects
{
    public class DocumentData : ValueObject
    {
        public string? Name { get; private set; }           // was product id + guid or sequence number
        public string? Title { get; private set; }
        public int SequenceNumber { get; private set; }
        public string? DocumentUrl { get; private set; }

        public DocumentData() { }

        public DocumentData(string name, string title, int sequenceNumber, string documentUrl)
        {
            Name = name;
            Title = title;
            SequenceNumber = sequenceNumber;
            DocumentUrl = documentUrl;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            // Using a yield return statement to return each element one at a time
            yield return Name!;
            yield return Title!;
            yield return SequenceNumber;
            yield return DocumentUrl!;
        }
    }
}
