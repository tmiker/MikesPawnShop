using Products.Write.Domain.Base;

namespace Products.Write.Domain.Events
{
    public class ImageDeleted : IDomainEvent
    {
        public Guid AggregateId { get; init; }
        public string AggregateType { get; init; } = default!;
        public int AggregateVersion { get; init; }
        public DateTime OccurredAt { get; init; }
        public string? CorrelationId { get; init; } 
        public string FileName { get; init; } = default!;

        public ImageDeleted(Guid aggregateId, string aggregateType, int aggregateVersion,
            string? correlationId, string fileName)
        {
            AggregateId = aggregateId;
            AggregateType = aggregateType;
            AggregateVersion = aggregateVersion;
            OccurredAt = DateTime.UtcNow;
            CorrelationId = correlationId;
            FileName = fileName;
        }
    }
}
