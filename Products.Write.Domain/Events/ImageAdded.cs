using Products.Write.Domain.Base;
using Products.Write.Domain.Enumerations;

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
        public string? Caption { get; private set; }
        public int SequenceNumber { get; private set; }
        public string? ImageUrl { get; private set; }
        public string? ThumbnailUrl { get; private set; }

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
