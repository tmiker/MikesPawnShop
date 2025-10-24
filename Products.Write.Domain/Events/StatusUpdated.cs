using Products.Write.Domain.Base;
using Products.Write.Domain.Enumerations;

namespace Products.Write.Domain.Events
{
    public class StatusUpdated : IDomainEvent
    {
        public Guid AggregateId { get; init; }
        public string AggregateType { get; init; } = default!;
        public int AggregateVersion { get; init; }
        public DateTime OccurredAt { get; init; }
        public string CorrelationId { get; init; } = default!;
        public string Status { get; init; } = default!;

        public StatusUpdated() { }

        public StatusUpdated(Guid aggregateId, string aggregateType, int aggregateVersion,
            string correlationId, string status)   // validate valid status from enumeration
        {
            AggregateId = aggregateId;
            AggregateType = aggregateType;
            AggregateVersion = aggregateVersion;
            OccurredAt = DateTime.UtcNow;
            CorrelationId = correlationId;
            Status = status;
        }
    }
}
