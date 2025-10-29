using Microsoft.EntityFrameworkCore;
using Products.Read.API.Abstractions;
using Products.Read.API.Domain.Models;
using Products.Read.API.Exceptions;
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
            else HandleProductStateSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!);
            return -1;
        }

        public async Task UpdateProductStatusAsync(StatusUpdatedMessage message)
        {
            Product? product = await _db.Products.FirstOrDefaultAsync(p => p.AggregateId == message.AggregateId);

            if (product is null) HandleProductIsNullSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!);
                
            product!.UpdateStatus(message.Status, message.AggregateVersion);
            bool success = await _db.SaveChangesAsync() > 0;

            if (!success) HandleProductStateSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!);
        }

        public async Task AddProductImageAsync(ImageAddedMessage message)
        {
            Product? product = await _db.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.AggregateId == message.AggregateId);

            if (product is null) HandleProductIsNullSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!);

            ImageData image = new ImageData(message.Name!, message.Caption!, message.SequenceNumber, message.ImageUrl!, message.ThumbnailUrl!);
            product!.AddImage(image, message.AggregateVersion);
            bool success = await _db.SaveChangesAsync() > 0;

            if (!success) HandleProductStateSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!);
        }

        public async Task AddProductDocumentAsync(DocumentAddedMessage message)
        {
            Product? product = await _db.Products.Include(p => p.Documents).FirstOrDefaultAsync(p => p.AggregateId == message.AggregateId);

            if (product is null) HandleProductIsNullSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!);

            DocumentData document = new DocumentData(message.Name!, message.Title!, message.SequenceNumber, message.DocumentUrl!);
            product!.AddDocument(document, message.AggregateVersion);
            bool success = await _db.SaveChangesAsync() > 0;

            if (!success) HandleProductStateSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!);
        }

        private void HandleProductIsNullSynchronizationError(string messageType, Guid aggregateId, string correlationId)
        {
            _logger.LogError("Error: Product associated with Write Side synchronization message was not found. " +
                "Message Type: {messageType}, AggregageId: {aggId}, CorrelationId: {corrId}", messageType, aggregateId, correlationId);
            throw new DataConsistencyException($"Product associated with Write Side synchronization message was not found. " +
                $"Message Type: {messageType}, AggregageId: {aggregateId}, CorrelationId: {correlationId}");
        }

        private void HandleProductStateSynchronizationError(string messageType, Guid aggregateId, string correlationId)
        {
            _logger.LogError("Error synchronizing product state from write side message. " +
                "Message Type: {messageType}, AggregageId: {aggId}, CorrelationId: {corrId}", messageType, aggregateId, correlationId);
            // return -1;
            throw new DataConsistencyException($"Error synchronizing product state from write side message. " +
                $"Message Type: {messageType}, AggregageId: {aggregateId}, CorrelationId: {correlationId}");
        }
    }
}
