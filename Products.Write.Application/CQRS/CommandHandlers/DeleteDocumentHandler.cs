using Microsoft.Extensions.Logging;
using Products.Write.Application.Abstractions;
using Products.Write.Application.CQRS.CommandResults;
using Products.Write.Application.CQRS.Commands;
using Products.Write.Domain.Aggregates;
using Products.Write.Infrastructure.Abstractions;

namespace Products.Write.Application.CQRS.CommandHandlers
{
    public class DeleteDocumentHandler : ICommandHandler<DeleteDocument, DeleteDocumentResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IAzureStorageService _azureStorageService;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogger<DeleteDocumentHandler> _logger;

        public DeleteDocumentHandler(IProductRepository productRepository, IAzureStorageService azureStorageService, IEventAggregator eventAggregator, ILogger<DeleteDocumentHandler> logger)
        {
            _productRepository = productRepository;
            _azureStorageService = azureStorageService;
            _eventAggregator = eventAggregator;
            _logger = logger;
        }

        public async Task<DeleteDocumentResult> HandleAsync(DeleteDocument command, CancellationToken cancellationToken)
        {
            Product product = await _productRepository.GetProductByIdAsync(command.ProductId);

            // delete image in domain
            product.DeleteDocument(command.FileName, command.CorrelationId);
            bool success = await _productRepository.SaveAsync(product);

            if (success)
            {
                // delete image from azure blob storage
                string containerName = $"product-{command.ProductId}";
                string fileName = command.FileName;
                (bool IsSuccess, string? ErrorMessage) result = await _azureStorageService.DeleteProductDocumentFromAzureAsync(containerName, fileName, cancellationToken);
                return new DeleteDocumentResult(result.IsSuccess, result.ErrorMessage);
            }

            return new DeleteDocumentResult(success, "Error removing document from write side data store");
        }
    }
}
