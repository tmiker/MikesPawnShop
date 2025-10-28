﻿using Microsoft.Extensions.Logging;
using Products.Write.Application.Abstractions;
using Products.Write.Application.CQRS.CommandResults;
using Products.Write.Application.CQRS.Commands;
using Products.Write.Domain.Aggregates;
using Products.Write.Infrastructure.Abstractions;

namespace Products.Write.Application.CQRS.CommandHandlers
{
    public class AddDocumentHandler : ICommandHandler<AddDocument, AddDocumentResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogger<AddDocumentHandler> _logger;

        public AddDocumentHandler(IProductRepository productRepository, IEventAggregator eventAggregator, ILogger<AddDocumentHandler> logger)
        {
            _productRepository = productRepository;
            _eventAggregator = eventAggregator;
            _logger = logger;
        }

        public async Task<AddDocumentResult> HandleAsync(AddDocument command, CancellationToken cancellationToken)
        {
            if (command.CorrelationId is null) command.CorrelationId = Guid.NewGuid().ToString();

            Product product = await _productRepository.GetProductByIdAsync(command.ProductId);
            product.AddDocument(command.Name, command.Title, command.SequenceNumber, command.DocumentUrl, command.CorrelationId);

            // Save the product - throws ProductEventStoreException if error occurs
            // THE REPOSITORY SHOULD ACTUALLY PROCESS ALL EVENTS TO THE EVENT STORE IN A SINGLE TRANSACTION, AND COMMIT ALL IN ONE GO SO WILL ROLL BACK IF ANY FAIL
            // THE BOOL SUCCESS SHOULD APPLY TO ALL EVENTS IN THE BATCH
            // MAY NOT WANT TO THROW AN EXCEPTION HERE - THOUGH EXCEPTION SHOULD BE HANDLED AT A HIGHER LEVEL RETURNING A PROBLEM DETAILS RESPONSE
            bool success = await _productRepository.SaveAsync(product);

            // Note, if have success, plus fact that event store will throw if error occurs, we can confidently assume success and publish product domain events
            if (success)
            {
                _logger.LogInformation("AddProductHandler created product with Id: {productId}. Command CorrelationId: {correlationId}", product.Id, command.CorrelationId);

                if (product.DomainEvents is not null && product.DomainEvents.Any())
                {
                    foreach (var domainEvent in product.DomainEvents)
                    {
                        // publish the event to the bus
                        _eventAggregator.Raise(domainEvent);
                    }
                }

                return new AddDocumentResult(true, null);
            }
            else return new AddDocumentResult(false, "An error occurred in persisting changes.");
        }
    }
}
