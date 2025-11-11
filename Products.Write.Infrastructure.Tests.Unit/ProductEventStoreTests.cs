using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Products.Write.Domain.Base;
using Products.Write.Domain.Enumerations;
using Products.Write.Domain.Events;
using Products.Write.Infrastructure.Abstractions;
using Products.Write.Infrastructure.Data;
using Products.Write.Infrastructure.DataAccess;
using Products.Write.Infrastructure.EventStores;
using System.Diagnostics.CodeAnalysis;

namespace Products.Write.Infrastructure
{
    public class ProductEventStoreTests
    {
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.None };

        [Fact]
        public async Task SaveEventRecordsAsync_ValidEventArg_CallsDbContextSave()
        {
            // Arrange
            bool saveResult = false;
            Guid aggregateId = Guid.NewGuid();

            IDomainEvent productAdded = new ProductAdded(aggregateId, "Product", 1, "Correlation Id", "Book Name", 
                CategoryEnum.Books, "Book Description", 25.99m, "USD", Status.Active.Name, 1, 1, "each", 1);
            string? expectedEventType = productAdded.GetType().AssemblyQualifiedName;
            string expectedEventJson = JsonConvert.SerializeObject(productAdded, _settings);

            var dbContextOptions = new DbContextOptionsBuilder<EventStoreDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            NullLogger<ProductEventStore> logger = NullLogger<ProductEventStore>.Instance;

            // Act
            // save event record by passing event to the method
            using (var context = new EventStoreDbContext(dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                IProductEventStore eventStore = new ProductEventStore(context, logger);
                IEnumerable<IDomainEvent> events = new List<IDomainEvent>() { productAdded };
                saveResult = await eventStore.SaveEventRecordsAsync(events);
            }

            // Assert
            // assert record is saved successfully to include all props
            using (var context = new EventStoreDbContext(dbContextOptions))
            {
                Assert.True(saveResult);
                EventRecord? recordFromDb = context.EventRecords.FirstOrDefault(u => u.AggregateId == aggregateId);
                Assert.Equal(productAdded.AggregateId, recordFromDb!.AggregateId);
                Assert.Equal(productAdded.AggregateType, recordFromDb!.AggregateType);
                Assert.Equal(productAdded.AggregateVersion, recordFromDb!.AggregateVersion);
                Assert.Equal(productAdded.OccurredAt, recordFromDb!.OccurredAt);
                Assert.Equal(productAdded.CorrelationId, recordFromDb!.CorrelationId);
                Assert.Equal(expectedEventType, recordFromDb!.EventType);
                Assert.Equal(expectedEventJson, recordFromDb!.EventJson);
            }
        }

        [Fact]
        public async Task GetDomainEventsByIdAsync_ValidAggregateIdArg_ReturnsAllEvents()
        {
            // Arrange
            bool saveResult = false;
            Guid aggregateId = Guid.NewGuid();
            IDomainEvent productAdded = new ProductAdded(aggregateId, "Product", 0, "Correlation Id 1", "Book Name",
                CategoryEnum.Books, "Book Description", 25.99m, "USD", Status.Active.Name, 1, 1, "each", 1);
            IDomainEvent statusUpdated = new StatusUpdated(aggregateId, "Product", 1, "Correlation Id 2", Status.InActive.Name);
            IDomainEvent imageAdded = new ImageAdded(aggregateId, "Product", 2, "Correlation Id 3", "Image Name", "Caption", 
                4, "Image URL", "Thumb URL");
            IDomainEvent docAdded = new DocumentAdded(aggregateId, "Product", 3, "Correlation Id 4", "Doc Name", "Title",
                3, "Doc URL");
            var expectedEventResult = new List<IDomainEvent>() { productAdded, statusUpdated, imageAdded, docAdded };
            // var expectedResult = new List<IDomainEvent>() { productAdded, imageAdded, docAdded };
            var dbContextOptions = new DbContextOptionsBuilder<EventStoreDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            NullLogger<ProductEventStore> logger = NullLogger<ProductEventStore>.Instance;

            // Act
            // save events as records then retrieve them from the event store to verify correctly returned
            using (var context = new EventStoreDbContext(dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                var eventStore = new ProductEventStore(context, logger);
                // saveResult = await eventStore.SaveAsEventRecordAsync(productAdded);
            }

            List<IDomainEvent> actualEventResult = new List<IDomainEvent>();
            List<OutboxRecord> actualOutboxResult = new List<OutboxRecord>();
            using (var context = new EventStoreDbContext(dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                IProductEventStore eventStore = new ProductEventStore(context, logger);

                IEnumerable<IDomainEvent> eventsToSave = new List<IDomainEvent>() { productAdded, statusUpdated, imageAdded, docAdded };
                saveResult = await eventStore.SaveEventRecordsAsync(eventsToSave);

                IEnumerable<IDomainEvent> eventsDeserializedFromStore = await eventStore.GetDomainEventsByIdAsync(aggregateId, 0, int.MaxValue);
                foreach (var evt in eventsDeserializedFromStore)
                {
                    actualEventResult.Add(evt);
                }
                actualOutboxResult = (await eventStore.GetOutboxRecordsAsync()).ToList();
            }

            // Assert
            // assert the expected and actual domain events are the same and outbox records created
            Assert.True(saveResult);
            Assert.Equal(expectedEventResult, actualEventResult, new DomainEventComparer());
            Assert.Equal(4, actualOutboxResult.Count);
            Assert.All(actualOutboxResult, o => Assert.Equal(aggregateId, o.AggregateId));
        }

        private class DomainEventComparer : IEqualityComparer<IDomainEvent>
        {
            public bool Equals(IDomainEvent? x, IDomainEvent? y)
            {
                if (x == null || y == null) return false;
                var event1 = (IDomainEvent)x;
                var event2 = (IDomainEvent)y;
                bool areEqual = true;
                if (event1.AggregateId != event2.AggregateId) areEqual = false;
                if (event1.GetType().Name != event2.GetType().Name) areEqual = false;
                if (event1 is ProductAdded pa1 && event2 is ProductAdded pa2)
                {
                    if (pa1.Category != pa2.Category) areEqual = false;
                }
                return areEqual;
            }

            public int GetHashCode([DisallowNull] IDomainEvent obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
