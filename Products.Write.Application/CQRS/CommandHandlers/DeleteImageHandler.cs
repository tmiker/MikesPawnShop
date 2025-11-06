using Microsoft.Extensions.Logging;
using Products.Write.Application.Abstractions;
using Products.Write.Application.CQRS.CommandResults;
using Products.Write.Application.CQRS.Commands;
using Products.Write.Domain.Aggregates;
using Products.Write.Domain.Snapshots;
using Products.Write.Infrastructure.Abstractions;

namespace Products.Write.Application.CQRS.CommandHandlers
{
    public class DeleteImageHandler : ICommandHandler<DeleteImage, DeleteImageResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IAzureStorageService _azureStorageService;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogger<DeleteImageHandler> _logger;

        public DeleteImageHandler(IProductRepository productRepository, IAzureStorageService azureStorageService, IEventAggregator eventAggregator, ILogger<DeleteImageHandler> logger)
        {
            _productRepository = productRepository;
            _azureStorageService = azureStorageService;
            _eventAggregator = eventAggregator;
            _logger = logger;
        }

        public async Task<DeleteImageResult> HandleAsync(DeleteImage command, CancellationToken cancellationToken)
        {
            // This needs to be called from a client page with images, get the filename as image.Name, use the product id, and delete
            // Currently deletes the first image.
            Product product = await _productRepository.GetProductByIdAsync(command.ProductId);

            //product.DeleteImage(command.FileName);
            //await _productRepository.SaveAsync(product);

            ProductSnapshot snapshot = product.GetSnapshot();
            if (snapshot.Images is not null && snapshot.Images.Any())
            {
                // need to change to use the filename from the command
                //var image = snapshot.Images.FirstOrDefault(i => i.Name == command.FileName);
                //if (image is not null)
                //{
                //    string containerName = $"product-{command.ProductId}";
                //    string fileName = "f15.jpg";    // image.Name!;
                //    (bool IsSuccess, string? ErrorMessage) result = await _azureStorageService.DeleteProductImageFromAzureAsync(containerName, fileName, cancellationToken);
                //    return new DeleteImageResult(result.IsSuccess, result.ErrorMessage);
                //}
                //return new DeleteImageResult(false, $"An image with filename {command.FileName} was not found.");

                string containerName = $"product-{command.ProductId}";
                string fileName = "f15.jpg"; // this works so need to include extension in filename   // image.Name!;
                (bool IsSuccess, string? ErrorMessage) result = await _azureStorageService.DeleteProductImageFromAzureAsync(containerName, fileName, cancellationToken);
                return new DeleteImageResult(result.IsSuccess, result.ErrorMessage);

            }
            return new DeleteImageResult(false, "The product has no images");
        }
    }
}