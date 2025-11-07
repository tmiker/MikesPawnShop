using Products.Write.Domain.Base;
using Products.Write.Domain.Enumerations;

namespace Products.Write.Domain.Events
{
    public class ProductAdded : IDomainEvent
    {
        public Guid AggregateId { get; init; }
        public string AggregateType { get; init; } = default!;
        public int AggregateVersion { get; init; }
        public DateTime OccurredAt { get; init; }
        public string? CorrelationId { get; init; } 

        public string Name { get; init; } = default!;
        public string Category { get; init; } = default!;
        public string Description { get; init; } = default!;
        public decimal Price { get; init; }
        public string Currency { get; init; } = default!;
        public string Status { get; init; } = default!;

        public ProductAdded(Guid aggregateId, string aggregateType, int aggregateVersion, 
            string? correlationId, string name, CategoryEnum category, 
            string description, decimal price, string currency, string status)
        {
            AggregateId = aggregateId;
            AggregateType = aggregateType;
            AggregateVersion = aggregateVersion;
            OccurredAt = DateTime.UtcNow;
            CorrelationId = correlationId;
            Name = name;
            Category = category.ToString();
            Description = description;
            Price = price;
            Currency = currency;
            Status = status;
        }
    }
}
