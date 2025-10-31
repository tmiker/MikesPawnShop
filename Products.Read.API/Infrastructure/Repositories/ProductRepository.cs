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
                // method GetCorrectProductAndVersionWithRetriesAsync handles null, missing versions, and duplicate messages
                Product? product = await GetCorrectProductAndVersionWithRetriesAsync(
                    message.GetType().Name, message.AggregateId, message.AggregateVersion, message.CorrelationId);

                // UPDATE EVEN IF STATUS THE SAME - STILL NEED TO PROCESS THE EVENT AND UPDATE THE VERSION
                product!.UpdateStatus(message.Status, message.AggregateVersion);
                bool success = await _db.SaveChangesAsync() > 0;

                // handle update error with no exception thrown
                if (!success) HandleProductStateSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!, null);
            }
            catch (DuplicateProductMessageException dupEx)
            {
                // just log for info
                _logger.LogInformation(dupEx.Message);
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
                // method GetCorrectProductAndVersionWithRetriesAsync handles null, missing versions, and duplicate messages
                Product? product = await GetCorrectProductAndVersionWithRetriesAsync(
                    message.GetType().Name, message.AggregateId, message.AggregateVersion, message.CorrelationId);

                ImageData image = new ImageData(message.Name!, message.Caption!, message.SequenceNumber, message.ImageUrl!, message.ThumbnailUrl!);
                product!.AddImage(image, message.AggregateVersion);
                bool success = await _db.SaveChangesAsync() > 0;

                // handle update error with no exception thrown
                if (!success) HandleProductStateSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!, null);

            }
            catch (DuplicateProductMessageException dupEx)
            {
                // just log for info
                _logger.LogInformation(dupEx.Message);
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
                // method GetCorrectProductAndVersionWithRetriesAsync handles null, missing versions, and duplicate messages
                Product? product = await GetCorrectProductAndVersionWithRetriesAsync(
                    message.GetType().Name, message.AggregateId, message.AggregateVersion, message.CorrelationId);

                DocumentData document = new DocumentData(message.Name!, message.Title!, message.SequenceNumber, message.DocumentUrl!);
                product!.AddDocument(document, message.AggregateVersion);
                bool success = await _db.SaveChangesAsync() > 0;

                // handle update error with no exception thrown
                if (!success) HandleProductStateSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!, null);

            }
            catch (DuplicateProductMessageException dupEx)
            {
                // just log for info
                _logger.LogInformation(dupEx.Message);
            }
            catch (Exception ex)    // likely a DbUpdateException or MissingProductVersionException
            {
                HandleProductStateSynchronizationError(message.GetType().Name, message.AggregateId, message.CorrelationId!, ex);
            }
        }

        private async Task<Product?> GetCorrectProductAndVersionWithRetriesAsync(string messageType, Guid aggregateId, int messageVersion, string? correlationId) //, 
        {
            int intervalSeconds = 5; 
            int retryCount = 3;
            int intervalMultiplier = 2;

            Product? product = null;
            while (retryCount > 0)
            {
                _logger.LogInformation("*********** PRODUCTS READ SIDE REPOSITORY ATTEMPTING TO GET CORRECT PRODUCT AND VERSION: ********** \n MESSAGE TYPE: {message_type}, " +
                "MESSAGE VERSION: {message_version}, AGGREGATE ID: {aggId}, CORRELATION ID: {corrId} RETRY COUNT: {retry} ...", messageType, messageVersion, aggregateId, correlationId, retryCount);

                product = await _db.Products.Include(p => p.Images).Include(p => p.Documents).AsSplitQuery().FirstOrDefaultAsync(p => p.AggregateId == aggregateId);

                if (product is not null)
                {
                    if (product.Version == messageVersion - 1) return product;
                    if (product.Version >= messageVersion)
                    {
                        // will catch and log this in process, then return as not a show stopper
                        throw new DuplicateProductMessageException($"Duplicate message: Version {messageVersion}, AggregateId: {aggregateId}");
                    }
                }

                // requery the database if the product is null, or if product is not null but product.Version < (messageVersion - 1)
                // as need to ensure finding the version that precedes the message version by 1
                intervalSeconds = intervalSeconds * intervalMultiplier;
                retryCount--;
                await Task.Delay(intervalSeconds * 1000);
            }

            if (product is null) HandleProductIsNullSynchronizationError(messageType, aggregateId, correlationId!);
            else if (product.Version < messageVersion - 1) HandleMissingProductMessageVersionError(messageType, aggregateId, messageVersion, correlationId!);

            return product;
        }

        private void HandleProductIsNullSynchronizationError(string messageType, Guid aggregateId, string correlationId)
        {
            _logger.LogError("Error: Product associated with Write Side synchronization message was not found. " +
                "Message Type: {messageType}, AggregageId: {aggId}, CorrelationId: {corrId}.", messageType, aggregateId, correlationId);
            throw new DataConsistencyException($"Product associated with Write Side synchronization message was not found. " +
                $"Message Type: {messageType}, AggregageId: {aggregateId}, CorrelationId: {correlationId}");
        }

        private void HandleMissingProductMessageVersionError(string messageType, Guid aggregateId, int messageVersion, string correlationId)
        {
            _logger.LogError("Missing Product Message Version {missingMessageVersion}. Unable to process {messageType} message " +
                "for AggregateId {aggregateId}, Version {messageVersion}, CorrelationId {correlationId} as the previous message " +
                "is missing.", messageVersion - 1, messageType, aggregateId, messageVersion, correlationId);
            throw new DataConsistencyException($"Error synchronizing product state from write side message. " +
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
