using Products.Write.Domain.Base;
using Products.Write.Infrastructure.Data;

namespace Products.Write.Infrastructure.Abstractions
{
    public interface IProductEventStore
    {
        Task<bool> SaveEventRecordsAsync(IEnumerable<IDomainEvent> events);

        // Task<bool> SaveAsEventRecordAsync(IDomainEvent @event);
        Task<IEnumerable<IDomainEvent>> GetDomainEventsByIdAsync(Guid aggregateId, int minVersion = 0, int maxVersion = Int32.MaxValue);
        Task<IEnumerable<OutboxRecord>> GetOutboxRecordsAsync();

        // DEV ONLY
        Task<IEnumerable<Guid>> GetUniqueAggregateIdsAsync();
        Task<bool> RemoveAllProductEventRecordsByIdAsync(Guid aggregateId);
        Task<bool> PurgeAllProductEventRecordsAsync();
    }
}
