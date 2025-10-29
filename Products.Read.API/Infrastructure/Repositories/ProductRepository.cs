using Microsoft.EntityFrameworkCore;
using Products.Read.API.Abstractions;
using Products.Read.API.Domain.Models;
using Products.Read.API.Infrastructure.Data;
using Products.Shared.Messages;

namespace Products.Read.API.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductsReadDbContext _db;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(ProductsReadDbContext db, ILogger<ProductRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<int> AddProductAsync(ProductAddedMessage message)
        {
            Product product = new Product
                (
                    aggregateId: message.AggregateId,
                    name: message.Name,
                    category: message.Category,
                    description: message.Description,
                    price: message.Price,
                    currency: message.Currency,
                    status: message.Status,
                    version: message.AggregateVersion
                );

            _db.Products.Add(product);
            bool success = await _db.SaveChangesAsync() > 0;
            if (success) return product.Id;
            _logger.LogError("Error adding product. AggregageId: {aggId} CorrelationId: {corrId}", message.AggregateId, message.CorrelationId);
            // return -1;
            throw new InvalidDataException($"Data inconsistency Error. Failed to add write side product. " +
                $"AggregageId: {message.AggregateId} CorrelationId: {message.CorrelationId}");
        }

        public async Task UpdateProductStatusAsync(StatusUpdatedMessage message)
        {
            Product? product = await _db.Products.FirstOrDefaultAsync(p => p.AggregateId == message.AggregateId);
            if (product is null)
            {
                _logger.LogError("Error finding product to update status. AggregageId: {aggId} CorrelationId: {corrId}", message.AggregateId, message.CorrelationId);
                throw new InvalidDataException($"Data inconsistency Error. Failed to find write side product to update status. " +
                    $"AggregageId: {message.AggregateId} CorrelationId: {message.CorrelationId}");
            }
            product.UpdateStatus(message.Status, message.AggregateVersion);
            bool success = await _db.SaveChangesAsync() > 0;
            if (!success)
            {
                _logger.LogError("Error updating product status. AggregageId: {aggId} CorrelationId: {corrId}", message.AggregateId, message.CorrelationId);
                throw new InvalidDataException($"Data inconsistency Error. Failed to update status of write side product. " +
                    $"AggregageId: {message.AggregateId} CorrelationId: {message.CorrelationId}");
            }
        }

        public async Task AddProductImageAsync(ImageAddedMessage message)
        {
            Product? product = await _db.Products.FirstOrDefaultAsync(p => p.AggregateId == message.AggregateId);
            if (product is null)
            {
                _logger.LogError("Error finding product to add image. AggregageId: {aggId} CorrelationId: {corrId}", message.AggregateId, message.CorrelationId);
                throw new InvalidDataException($"Data inconsistency Error. Failed to find write side product to add image. " +
                    $"AggregageId: {message.AggregateId} CorrelationId: {message.CorrelationId}");
            }
                
            ImageData image = new ImageData(message.Name!, message.Caption!, message.SequenceNumber, message.ImageUrl!, message.ThumbnailUrl!);
            product.AddImage(image, message.AggregateVersion);
            bool success = await _db.SaveChangesAsync() > 0;
            if (!success)
            {
                _logger.LogError("Error adding image to product. AggregageId: {aggId} CorrelationId: {corrId}", message.AggregateId, message.CorrelationId);
                throw new InvalidDataException($"Data inconsistency Error. Failed to add image to write side product. " +
                    $"AggregageId: {message.AggregateId} CorrelationId: {message.CorrelationId}");
            }
        }

        public async Task AddProductDocumentAsync(DocumentAddedMessage message)
        {
            Product? product = await _db.Products.Include(p => p.Documents).FirstOrDefaultAsync(p => p.AggregateId == message.AggregateId);
            if (product is null)
            {
                _logger.LogError("Error finding product to add document. AggregageId: {aggId} CorrelationId: {corrId}", message.AggregateId, message.CorrelationId);
                throw new InvalidDataException($"Data inconsistency Error. Failed to find write side product to add document. " +
                    $"AggregageId: {message.AggregateId} CorrelationId: {message.CorrelationId}");
            }

            DocumentData document = new DocumentData(message.Name!, message.Title!, message.SequenceNumber, message.DocumentUrl!);
            product.AddDocument(document, message.AggregateVersion);
            bool success = await _db.SaveChangesAsync() > 0;
            if (!success)
            {
                _logger.LogError("Error adding document to product. AggregageId: {aggId} CorrelationId: {corrId}", message.AggregateId, message.CorrelationId);
                // return -1;
                throw new InvalidDataException($"Data inconsistency Error. Failed to add document to write side product. " +
                    $"AggregageId: {message.AggregateId} CorrelationId: {message.CorrelationId}");
            }
        }
    }
}
