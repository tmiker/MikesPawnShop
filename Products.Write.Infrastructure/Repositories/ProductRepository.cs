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

        public async Task SaveAsync(Product product)
        {
            if (product.DomainEvents is null || !product.DomainEvents.Any()) return;
            IEnumerable<IDomainEvent> events = product.DomainEvents;
            foreach (var @event in events)
            {
                await _eventStore.SaveAsEventRecordAsync(@event);
            }
            _logger.LogInformation("Product Repository sent {count} events to the Event Store", events.Count());
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
