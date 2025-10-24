namespace Products.Write.Domain.Base
{
    public interface IDomainEvent
    {
        Guid AggregateId { get; }
        string AggregateType { get; }
        int AggregateVersion { get; }
        DateTime OccurredAt { get; }
        string CorrelationId { get; }
    }
}
