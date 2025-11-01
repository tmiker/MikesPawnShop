namespace Products.Shared.Abstractions
{
    public interface IProductMessage
    {
        Guid AggregateId { get;  }
        // string AggregateType { get; }
        int AggregateVersion { get; init; }
        // DateTime OccurredAt { get; init; }
        string? CorrelationId { get; init; } 
    }
}
