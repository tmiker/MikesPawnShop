using Microsoft.Extensions.Logging;
using Products.Write.Application.Abstractions;
using Products.Write.Application.CQRS.CommandResults;
using Products.Write.Application.CQRS.Commands;
using Products.Write.Domain.Aggregates;
using Products.Write.Domain.Snapshots;
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
            // This needs to be called from a client page with images, get the filename as image.Name, use the product id, and delete
            // Currently deletes the first image.
            Product product = await _productRepository.GetProductByIdAsync(command.ProductId);

            //product.DeleteDocument(command.FileName);
            //await _productRepository.SaveAsync(product);

            ProductSnapshot snapshot = product.GetSnapshot();
            if (snapshot.Documents is not null && snapshot.Documents.Any())
            {
                // need to change to use the filename from the command
                var doc = snapshot.Documents.First();
                string containerName = $"product-{command.ProductId}";
                string fileName = doc.Name!;
                (bool IsSuccess, string? ErrorMessage) result = await _azureStorageService.DeleteProductDocumentFromAzureAsync(containerName, fileName, cancellationToken);
                return new DeleteDocumentResult(result.IsSuccess, result.ErrorMessage);

            }
            return new DeleteDocumentResult(false, "The product has no documents");
        }
    }
}
