using MassTransit;
using Microsoft.Extensions.Logging;
using Products.Shared.Messages;
using Products.Write.Application.Abstractions;
using Products.Write.Application.CQRS.CommandResults;
using Products.Write.Application.CQRS.Commands;
using Products.Write.Domain.Aggregates;
using Products.Write.Domain.Enumerations;
using Products.Write.Infrastructure.Abstractions;

namespace Products.Write.Application.CQRS.CommandHandlers
{
    public class AddProductHandler : ICommandHandler<AddProduct, AddProductResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogger<AddProductHandler> _logger;

        public AddProductHandler(IProductRepository productRepository, IEventAggregator eventAggregator, ILogger<AddProductHandler> logger)
        {
            _productRepository = productRepository;
            _eventAggregator = eventAggregator;
            _logger = logger;
        }

        public async Task<AddProductResult> HandleAsync(AddProduct command, CancellationToken cancellationToken)
        {
            if (command.CorrelationId is null) command.CorrelationId = Guid.NewGuid().ToString();
            CategoryEnum categoryEnum = (CategoryEnum)Enum.Parse(typeof(CategoryEnum), command.Category, ignoreCase: true);

            Product product = new Product(command.Name, categoryEnum, command.Description, command.Price, command.Currency, command.Status, command.CorrelationId);
            Guid productId = product.Id;

            // Save the product - throws ProductEventStoreException if error occurs
            if (!cancellationToken.IsCancellationRequested)
            {
                await _productRepository.SaveAsync(product);

                // Because will throw if error occurs, we can assume success and publish product domain events
                _logger.LogInformation("AddProductHandler created product with Id: {productId}. Command CorrelationId: {correlationId}", productId, command.CorrelationId);

                if (product.DomainEvents is not null && product.DomainEvents.Any())
                {
                    foreach (var domainEvent in product.DomainEvents)
                    {
                        _eventAggregator.Raise(domainEvent);
                    }
                }

                return new AddProductResult(true, productId, null);
            }

            return new AddProductResult(false, Guid.Empty, "Operation cancelled.");
        }
    }
}

// ENUM.PARSE EXCEPTIONS:

//ArgumentNullException
//enumType is null.

//ArgumentException
//enumType is not an Enum.

//ArgumentException
//value is either an empty string or only contains white space.

//ArgumentException
//value is a name, but not one of the named constants defined for the enumeration.

//OverflowException
//value is outside the range of the underlying type of enumType.

//InvalidOperationException
//.NET 8 and later versions: enumType is a Boolean - backed enumeration type.