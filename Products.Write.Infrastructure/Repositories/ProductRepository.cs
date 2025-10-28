using Microsoft.Extensions.Logging;
using Products.Write.Domain.Aggregates;
using Products.Write.Domain.Base;
using Products.Write.Infrastructure.Abstractions;

namespace Products.Write.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IProductEventStore _eventStore;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(IProductEventStore eventStore, ILogger<ProductRepository> logger)
        {
            _eventStore = eventStore;
            _logger = logger;
        }

        // REFACTOR TRANSACTIONS

        // Separating persistence of Domain Events into two method calls ???
        //   - e.g. StageAggregateEventsAsync - EventStore.StageAggregateEventsAsync + SaveAsync - EventStore.SaveChangesAsync ???
        //      - No, should do all in a single method (add event records and commit transaction) for user clarity
        //      - This is for persisting the aggregate state in the form of event records, ...
        //          ... i.e. a single aggregate only - doing Event SOURCING, not Event DRIVEN Architecture  
        //      - Events are the persistence mechanism for the aggregate, not for inter-service communication
        //      - For Event Driven architecture would need a service encapsulating the command handler, ...
        //          ... publishing these events so can have side effects on other aggregates, ...
        //          ... with a method to be called to integrate multiple transactions across different aggregates if needed.

        public async Task<bool> SaveAsync(Product product)
        {
            if (product.DomainEvents != null && product.DomainEvents.Any())
            {
                bool success = await _eventStore.SaveEventRecordsAsync(product.DomainEvents);
                return success; // error handling occurs in event store
            }
            else
            {
                _logger.LogInformation("Product Repository found no events to send to the Event Store for Product Id: {productId}", product.Id);
                return true; // nothing to save, but not an error
            }

            //bool success = true;
            //if (product.DomainEvents != null && product.DomainEvents.Any())
            //{
            //    // THIS SHOULD ACTUALLY SEND ALL EVENTS TO THE EVENT STORE IN A TRANSACTION, AND COMMIT ALL IN ONE GO SO WILL ROLL BACK IF ANY FAIL
            //    // THE BOOL SUCCESS SHOULD APPLY TO ALL EVENTS IN THE BATCH
            //    foreach (var domainEvent in product.DomainEvents)
            //    {
            //        bool recordSaved = await _eventStore.SaveAsEventRecordAsync(domainEvent);
            //        if (!recordSaved) success = false;
            //    }
            //}

            //_logger.LogInformation("Product Repository sent {count} events to the Event Store", product.DomainEvents?.Count);
            //return success;
        }

        public async Task<Product> GetProductByIdAsync(Guid aggregateId)
        {
            IEnumerable<IDomainEvent> events = await _eventStore.GetDomainEventsByIdAsync(aggregateId, 0, Int32.MaxValue);
            Product product = new Product(events);
            return product;
        }

        public async Task<Product> GetProductByIdAndVersionAsync(Guid aggregateId, int minVersion, int maxVersion)
        {
            IEnumerable<IDomainEvent> events = await _eventStore.GetDomainEventsByIdAsync(aggregateId, minVersion, maxVersion);
            Product product = new Product(events);
            return product;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            IEnumerable<Guid> uniqueAggregateIds = await _eventStore.GetUniqueAggregateIdsAsync();
            ICollection<Product> products = new List<Product>();
            foreach (var aggregateId in uniqueAggregateIds)
            {
                IEnumerable<IDomainEvent> domainEvents = await _eventStore.GetDomainEventsByIdAsync(aggregateId);

                if (domainEvents.Any())
                {
                    Product product = new Product(domainEvents);
                    products.Add(product);
                }
            }
            return products;
        }
    }
}
