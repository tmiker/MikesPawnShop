using Microsoft.Extensions.Logging;
using Products.Write.Application.Abstractions;
using Products.Write.Application.CQRS.CommandResults;
using Products.Write.Application.CQRS.Commands;
using Products.Write.Domain.Aggregates;
using Products.Write.Domain.Enumerations;
using Products.Write.Infrastructure.Abstractions;

namespace Products.Write.Application.CQRS.CommandHandlers
{
    public class ProcessMultipleEventsHandler : ICommandHandler<ProcessMultipleEvents, ProcessMultipleEventsResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogger<ProcessMultipleEventsHandler> _logger;

        public ProcessMultipleEventsHandler(IProductRepository productRepository, IEventAggregator eventAggregator, ILogger<ProcessMultipleEventsHandler> logger)
        {
            _productRepository = productRepository;
            _eventAggregator = eventAggregator;
            _logger = logger;
        }

        public async Task<ProcessMultipleEventsResult> HandleAsync(ProcessMultipleEvents command, CancellationToken cancellationToken)
        {
            if (command.CorrelationId is null) command.CorrelationId = Guid.NewGuid().ToString();

            Product product = new Product("Product 1", CategoryEnum.Books, "A book on things.", 25.99m, "USD", "Active", command.CorrelationId);
            product.UpdateStatus("InActive", command.CorrelationId);
            product.AddImage("Image 1", "A dog", 3, "Image URL", "Thumb URL", command.CorrelationId);
            product.AddDocument("Doc 1", "Instructions", 1, "Document URL", command.CorrelationId);

            bool success = await _productRepository.SaveAsync(product);

            // Note, if have success, plus fact that event store will throw if error occurs, we can confidently assume success and publish product domain events
            if (success)
            {
                _logger.LogInformation("ProcessMultipleEventsHandler handled multiple events for product with Id: {productId}. Command CorrelationId: {correlationId}", product.Id, command.CorrelationId);

                if (product.DomainEvents is not null && product.DomainEvents.Any())
                {
                    foreach (var domainEvent in product.DomainEvents)
                    {
                        // publish the event to the bus
                        _eventAggregator.Raise(domainEvent);
                    }
                }

                return new ProcessMultipleEventsResult(true, null);
            }
            else return new ProcessMultipleEventsResult(false, "An error occurred in persisting changes.");
        }
    }
}
