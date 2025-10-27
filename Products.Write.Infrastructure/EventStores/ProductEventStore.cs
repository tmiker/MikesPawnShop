using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Products.Write.Domain.Base;
using Products.Write.Infrastructure.Abstractions;
using Products.Write.Infrastructure.Data;
using Products.Write.Infrastructure.DataAccess;
using Products.Write.Infrastructure.Exceptions;

namespace Products.Write.Infrastructure.EventStores
{
    public class ProductEventStore : IProductEventStore
    {
        // private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings() { TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full };
        private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.None };

        private readonly EventStoreDbContext _eventStoreDbContext;
        private readonly ILogger<ProductEventStore> _logger;

        public ProductEventStore(EventStoreDbContext eventStoreDbContext, ILogger<ProductEventStore> logger)
        {
            _eventStoreDbContext = eventStoreDbContext;
            _logger = logger;
        }

        public async Task<bool> SaveAsEventRecordAsync(IDomainEvent @event)
        {
            EventRecord eventRecord = new EventRecord(
                @event.AggregateId,
                @event.AggregateType,
                @event.AggregateVersion,
                @event.GetType().AssemblyQualifiedName ?? throw new InvalidDataException("Invalid Event Type"),
                JsonConvert.SerializeObject(@event, _jsonSettings),
                @event.OccurredAt,
                @event.CorrelationId); 

            _eventStoreDbContext.EventRecords.Add(eventRecord);
            // create outbox record from event record and add to outbox records - retain atomicity without using UOW
            OutboxRecord outboxRecord = new OutboxRecord(eventRecord);
            // _eventStoreDbContext.OutboxRecords.Add(outboxRecord);

            bool success = await _eventStoreDbContext.SaveChangesAsync() > 0;
            if (!success)
            {
                _logger.LogError("Error saving event as event record along with an outbox record. Aggregate Type: {agg_type}, Aggregate Id: {agg_id}, Correlation Id {corr_id}", @event.AggregateType, @event.AggregateId, @event.CorrelationId);
                throw new ProductEventStoreException("Error saving event as event record.");
            }
            return success;
        }

        public async Task<IEnumerable<IDomainEvent>> GetDomainEventsByIdAsync(Guid aggregateId, int minVersion = 0, int maxVersion = Int32.MaxValue)
        {
            IEnumerable<EventRecord> records = await _eventStoreDbContext.EventRecords.Where(
                r => r.AggregateId == aggregateId && 
                r.AggregateVersion >= minVersion  && 
                r.AggregateVersion <= maxVersion).ToListAsync();

            List<IDomainEvent> events = new List<IDomainEvent>();
            foreach (var record in records)
            {
                var eventObject = JsonConvert.DeserializeObject(record.EventJson, Type.GetType(record.EventType)!);
                if (eventObject is null)
                {
                    _logger.LogError("Error deserializing domain event object from event record. Aggregate Type: {agg_type}, Aggregate Id: {agg_id}, Correlation Id {corr_id}", record.AggregateType, record.AggregateId, record.CorrelationId);
                    throw new ProductEventStoreException("Error deserializing domain event object from event record.");
                }
                var @event = (IDomainEvent)eventObject!;
                if (@event is null)
                {
                    _logger.LogError("Error casting to domain event from object. Aggregate Type: {agg_type}, Aggregate Id: {agg_id}, Correlation Id {corr_id}", record.AggregateType, record.AggregateId, record.CorrelationId);
                    throw new ProductEventStoreException("Error casting to domain event from object.");
                }
                else events.Add(@event);
            }

            return events;
        }

        // DEV ONLY
        public async Task<IEnumerable<Guid>> GetUniqueAggregateIdsAsync()
        {
            IEnumerable<Guid> uniqueIds = await _eventStoreDbContext.EventRecords
                .Select(r => r.AggregateId)
                .Distinct().ToListAsync();

            return uniqueIds;
        }

        public async Task<bool> RemoveAllProductEventRecordsByIdAsync(Guid aggregateId)
        {
            IEnumerable<EventRecord> records = await _eventStoreDbContext.EventRecords.Where(
                r => r.AggregateId == aggregateId &&
                r.AggregateVersion >= 0 &&
                r.AggregateVersion <= Int32.MaxValue).ToListAsync();

            _eventStoreDbContext.EventRecords.RemoveRange(records);
            int count = await _eventStoreDbContext.SaveChangesAsync();
            _logger.LogInformation("Product Event Store records for Aggregate Id {aggregateId} removed. Rows deleted: {count}", aggregateId, count);
            return count > 0;
        }

        public async Task<bool> PurgeAllProductEventRecordsAsync()
        {
            // EF CORE
            int count = await _eventStoreDbContext.EventRecords.ExecuteDeleteAsync();
            _logger.LogInformation("Product Event Store records were purged. Rows deleted: {count}", count);
            return count > 0;
        }
    }
}