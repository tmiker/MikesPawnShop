using Products.Write.Domain.Base;

namespace Products.Write.Domain.Events
{
    public class ImageAdded : IDomainEvent
    {
        public Guid AggregateId { get; init; }
        public string AggregateType { get; init; } = default!;
        public int AggregateVersion { get; init; }
        public DateTime OccurredAt { get; init; }
        public string CorrelationId { get; init; } = default!;
        public string Name { get; init; } = default!;
        public string Caption { get; init; } = default!;
        public int SequenceNumber { get; init; }
        public string ImageUrl { get; init; } = default!;
        public string ThumbnailUrl { get; init; } = default!;

        public ImageAdded(Guid aggregateId, string aggregateType, int aggregateVersion,
            string correlationId, string name, string caption, int sequenceNumber,
            string imageUrl, string thumbnailUrl)
        {
            AggregateId = aggregateId;
            AggregateType = aggregateType;
            AggregateVersion = aggregateVersion;
            OccurredAt = DateTime.UtcNow;
            CorrelationId = correlationId;
            Name = name;
            Caption = caption;
            SequenceNumber = sequenceNumber;
            ImageUrl = imageUrl;
            ThumbnailUrl = thumbnailUrl;
        }
    }
}
