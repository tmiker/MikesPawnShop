using Microsoft.Extensions.Logging;
using Products.Write.Application.Abstractions;
using Products.Write.Application.CQRS.CommandResults;
using Products.Write.Application.CQRS.Commands;
using Products.Write.Application.DTOs;
using Products.Write.Domain.Aggregates;
using Products.Write.Domain.Enumerations;
using Products.Write.Infrastructure.Abstractions;

namespace Products.Write.Application.CQRS.CommandHandlers
{
    public class ProcessMultipleEventsHandler : ICommandHandler<ProcessMultipleEvents, ProcessMultipleEventsResult>
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IProductRepository _productRepository;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogger<ProcessMultipleEventsHandler> _logger;

        public ProcessMultipleEventsHandler(ICommandDispatcher commandDispatcher, IProductRepository productRepository, IEventAggregator eventAggregator, ILogger<ProcessMultipleEventsHandler> logger)
        {
            _commandDispatcher = commandDispatcher;
            _productRepository = productRepository;
            _eventAggregator = eventAggregator;
            _logger = logger;
        }

        public async Task<ProcessMultipleEventsResult> HandleAsync(ProcessMultipleEvents command, CancellationToken cancellationToken)
        {
            if (command.CorrelationId is null) command.CorrelationId = Guid.NewGuid().ToString();

            try
            {
                AddProductDTO addProductDTO = new AddProductDTO("Product 1", CategoryEnum.Books.ToString(), "A book on things.", 25.99m, "USD", "Active");
                AddProduct addProductCommand = new AddProduct(addProductDTO, command.CorrelationId);
                AddProductResult addProductResult = await _commandDispatcher.DispatchAsync<AddProduct, AddProductResult>(addProductCommand, cancellationToken);

                Guid aggregateId = addProductResult.ProductId;

                UpdateStatus updateStatusCommand = new UpdateStatus() { ProductId = aggregateId, Status = Status.InActive.Name, CorrelationId = command.CorrelationId };
                UpdateStatusResult updateStatusResult = await _commandDispatcher.DispatchAsync<UpdateStatus, UpdateStatusResult>(updateStatusCommand, cancellationToken);

                AddImage addImageCommand = new AddImage() { ProductId = aggregateId, Name = "Image 1", Caption = "A dog", SequenceNumber = 3, ImageUrl = "Image URL", ThumbnailUrl = "Thumb URL", CorrelationId = command.CorrelationId };
                AddImageResult addImageResult = await _commandDispatcher.DispatchAsync<AddImage, AddImageResult>(addImageCommand, cancellationToken);

                AddDocument addDocumentCommand = new AddDocument() { ProductId = aggregateId, Name = "Document 1", Title = "Instructions", SequenceNumber = 3, DocumentUrl = "Document URL", CorrelationId = command.CorrelationId };
                AddDocumentResult addDocumentResult = await _commandDispatcher.DispatchAsync<AddDocument, AddDocumentResult>(addDocumentCommand, cancellationToken);

                _logger.LogInformation("ProcessMultipleEventsHandler handled multiple events for product with Id: {productId}. Command CorrelationId: {correlationId}", aggregateId, command.CorrelationId);

                return new ProcessMultipleEventsResult(true, null);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred in persisting changes.");
                return new ProcessMultipleEventsResult(false, ex.Message);
            }
        }
    }
}
