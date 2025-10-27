namespace Products.Shared.Messages
{
    public class DocumentAddedMessage
    {
        // for logging purposes on read side
        public Guid AggregateId { get; init; }
        public string AggregateType { get; init; } = default!;
        public int AggregateVersion { get; init; }
        public DateTime OccurredAt { get; init; }
        public string? CorrelationId { get; init; } // = default!;
        // command data
        public string? Name { get; init; } // = default!;
        public string? Title { get; init; } // = default!;
        public int SequenceNumber { get; init; }
        public string? DocumentUrl { get; init; } // = default!;

        public DocumentAddedMessage(Guid aggregateId, string aggregateType, int aggregateVersion,
            string correlationId, string name, string title, int sequenceNumber,
            string documentUrl)
        {
            AggregateId = aggregateId;
            AggregateType = aggregateType;
            AggregateVersion = aggregateVersion;
            OccurredAt = DateTime.UtcNow;
            CorrelationId = correlationId;
            Name = name;
            Title = title;
            SequenceNumber = sequenceNumber;
            DocumentUrl = documentUrl;
        }
    }
}
