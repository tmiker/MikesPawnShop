using Products.Write.Domain.Base;

namespace Products.Write.Infrastructure.Abstractions
{
    public interface IProductEventStore
    {
        Task<bool> SaveAsEventRecordAsync(IDomainEvent @event);
        Task<IEnumerable<IDomainEvent>> GetDomainEventsByIdAsync(Guid aggregateId, int minVersion = 0, int maxVersion = Int32.MaxValue);

        // DEV ONLY
        Task<IEnumerable<Guid>> GetUniqueAggregateIdsAsync();
        Task<bool> RemoveAllProductEventRecordsByIdAsync(Guid aggregateId);
        Task<bool> PurgeAllProductEventRecordsAsync();
    }
}
