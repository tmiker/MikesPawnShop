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

        public async Task<bool> SaveAsync(Product product)
        {
            bool success = true;
            if (product.DomainEvents != null && product.DomainEvents.Any())
            {
                foreach (var domainEvent in product.DomainEvents)
                {
                    bool recordSaved = await _eventStore.SaveAsEventRecordAsync(domainEvent);
                    if (!recordSaved) success = false;
                }
            }

            _logger.LogInformation("Product Repository sent {count} events to the Event Store", product.DomainEvents?.Count);
            return success;
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
