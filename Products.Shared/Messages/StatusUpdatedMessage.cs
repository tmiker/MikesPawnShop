namespace Products.Shared.Messages
{
    public class StatusUpdatedMessage
    {
        // for logging purposes on read side
        public Guid AggregateId { get; init; }
        public string AggregateType { get; init; } = default!;
        public int AggregateVersion { get; init; }
        public DateTime OccurredAt { get; init; }
        public string? CorrelationId { get; init; } // = default!; 
        // command data
        public string Status { get; init; } = default!;

        public StatusUpdatedMessage(Guid aggregateId, string aggregateType, int aggregateVersion,
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
