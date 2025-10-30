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

        public async Task AddProductAsync(ProductAddedMessage message)
        {
            try
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
                if (success) return;
                // handle update error with no exception thrown
                else HandleProductStateSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!, null);
            }
            catch (Exception ex)    // likely a DbUpdateException
            {
                HandleProductStateSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!, ex);
            }
        }

        public async Task UpdateProductStatusAsync(StatusUpdatedMessage message)
        {
            try
            {
                Product? product = await GetProductAndVersionWithRetriesAsync(message.AggregateId, message.AggregateVersion, 5, 3, 2);
                if (product is null)
                {
                    // May want to send to a queue to reprocess later, retry, or throw exception ...
                    HandleProductIsNullSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!);
                }
                else if (message.AggregateVersion <= product.Version) return;    // idempotency, duplicate command
                else if (message.AggregateVersion > product.Version + 1)         // version missing 
                {
                    // holding queue, or retry get product - BUT COULD CREATE A LOOP - BETTER TO USE A QUEUE AND LOG A WARNING OR ERROR ...
                    product = await GetProductAndVersionWithRetriesAsync(message.AggregateId, message.AggregateVersion, 30, 3, 2);
                }
                else  // this is the desired product, i.e. message.AggregateVersion == product.Version + 1; 
                {
                    // UPDATE EVEN IF STATUS THE SAME - STILL NEED TO PROCESS THE EVENT AND UPDATE THE VERSION
                    product!.UpdateStatus(message.Status, message.AggregateVersion);
                    bool success = await _db.SaveChangesAsync() > 0;

                    // handle update error with no exception thrown
                    if (!success) HandleProductStateSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!, null);
                } 
            }
            catch (Exception ex)    // likely a DbUpdateException
            {
                HandleProductStateSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!, ex);
            }
        }

        public async Task AddProductImageAsync(ImageAddedMessage message)
        {
            try
            {
                Product? product = await GetProductAndVersionWithRetriesAsync(message.AggregateId, message.AggregateVersion, 5, 3, 2);
                if (product is null)
                {
                    // May want to send to a queue to reprocess later, retry, or throw exception ...
                    HandleProductIsNullSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!);
                }
                else if (message.AggregateVersion <= product.Version) return;    // idempotency, duplicate command
                else if (message.AggregateVersion > product.Version + 1)         // version missing 
                {
                    // holding queue, or retry get product - BUT COULD CREATE A LOOP - BETTER TO USE A QUEUE AND LOG A WARNING OR ERROR ...
                    product = await GetProductAndVersionWithRetriesAsync(message.AggregateId, message.AggregateVersion, 30, 3, 2);
                }
                else  // this is the desired product, i.e. message.AggregateVersion == product.Version + 1; 
                {
                    ImageData image = new ImageData(message.Name!, message.Caption!, message.SequenceNumber, message.ImageUrl!, message.ThumbnailUrl!);
                    product!.AddImage(image, message.AggregateVersion);
                    bool success = await _db.SaveChangesAsync() > 0;

                    // handle update error with no exception thrown
                    if (!success) HandleProductStateSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!, null);
                }
            }
            catch (Exception ex)    // likely a DbUpdateException
            {
                HandleProductStateSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!, ex);
            }
        }

        public async Task AddProductDocumentAsync(DocumentAddedMessage message)
        {
            try
            {
                Product? product = await GetProductAndVersionWithRetriesAsync(message.AggregateId, message.AggregateVersion, 5, 3, 2);
                if (product is null)
                {
                    // May want to send to a queue to reprocess later, retry, or throw exception ...
                    HandleProductIsNullSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!);
                }
                else if (message.AggregateVersion <= product.Version) return;    // idempotency, duplicate command
                else if (message.AggregateVersion > product.Version + 1)         // version missing 
                {
                    // holding queue, or retry get product - BUT COULD CREATE A LOOP - BETTER TO USE A QUEUE AND LOG A WARNING OR ERROR ...
                    product = await GetProductAndVersionWithRetriesAsync(message.AggregateId, message.AggregateVersion, 30, 3, 2);
                }
                else  // this is the desired product, i.e. message.AggregateVersion == product.Version + 1; 
                {
                    DocumentData document = new DocumentData(message.Name!, message.Title!, message.SequenceNumber, message.DocumentUrl!);
                    product!.AddDocument(document, message.AggregateVersion);
                    bool success = await _db.SaveChangesAsync() > 0;

                    // handle update error with no exception thrown
                    if (!success) HandleProductStateSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!, null);
                }
            }
            catch (Exception ex)    // likely a DbUpdateException
            {
                HandleProductStateSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!, ex);
            }
        }

        private async Task<Product?> GetProductAndVersionWithRetriesAsync(Guid aggregateId, int messageVersion, int intervalSeconds, int retryCount, int multiplier)
        {
            Product? product = null;
            while (retryCount > 0)
            {
                product = await _db.Products.FirstOrDefaultAsync(p => p.AggregateId == aggregateId);
                if (product is not null)
                {
                    // if product.Version == messageVersion - 1 this is the correct product version to update
                    // if product.Version >= messageVersion the command is a duplicate - COULD THROW CUSTOM EXCEPTION AND CATCH IT ABOVE TO RETURN OUT OF ENCLOSING METHOD
                    if (product.Version == messageVersion - 1 || product.Version >= messageVersion) return product;
                }
                else
                {
                    intervalSeconds = intervalSeconds * multiplier;
                    retryCount--;
                    await Task.Delay(intervalSeconds * 1000);
                }
            }
            return product;
        }

        private void HandleProductIsNullSynchronizationError(string messageType, Guid aggregateId, string correlationId)
        {
            _logger.LogError("Error: Product associated with Write Side synchronization message was not found. " +
                "Message Type: {messageType}, AggregageId: {aggId}, CorrelationId: {corrId}.", messageType, aggregateId, correlationId);
            throw new DataConsistencyException($"Product associated with Write Side synchronization message was not found. " +
                $"Message Type: {messageType}, AggregageId: {aggregateId}, CorrelationId: {correlationId}");
        }

        private void HandleProductStateSynchronizationError(string messageType, Guid aggregateId, string correlationId, Exception? ex)
        {
            _logger.LogError("Error synchronizing product state from Write Side synchronization message. " +
                "Message Type: {messageType}, AggregageId: {aggId}, CorrelationId: {corrId}.  Exception: {ex}", messageType, aggregateId, correlationId, ex);
            // return -1;
            throw new DataConsistencyException($"Error synchronizing product state from write side message. " +
                $"Message Type: {messageType}, AggregageId: {aggregateId}, CorrelationId: {correlationId}");
        }
    }
}
