using Products.Shared.Abstractions;

namespace Products.Shared.Messages
{
    public class DataPurgedMessage : IProductMessage
    {
        public Guid AggregateId { get; } = Guid.Empty;
        // string AggregateType { get; }
        public int AggregateVersion { get; init; } = -1;
        // DateTime OccurredAt { get; init; }
        public string? CorrelationId { get; init; } = "0";
    }
}
