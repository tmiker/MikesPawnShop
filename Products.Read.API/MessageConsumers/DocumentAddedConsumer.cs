using MassTransit;
using Products.Read.API.Abstractions;
using Products.Shared.Messages;

namespace Products.Read.API.MessageConsumers
{
    public class DocumentAddedConsumer : IConsumer<DocumentAddedMessage>
    {
        private readonly IProductMessageProcessor _messageProcessor;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<DocumentAddedConsumer> _logger;

        public DocumentAddedConsumer(IProductMessageProcessor messageProcessor, IProductRepository productRepository, ILogger<DocumentAddedConsumer> logger)
        {
            _messageProcessor = messageProcessor;
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DocumentAddedMessage> context)
        {
            var message = context.Message;
            _logger.LogInformation("Document Added Message Received: VERSION = {version}, AggregateId = {message.AggregateId}, " +
                "Title = {message.Title}", message.AggregateVersion, message.AggregateId, message.Title);

            // await _productRepository.AddProductDocumentAsync(message);

            bool messagesInMessageRecordQueue = await _messageProcessor.ProcessProductMessageAsync(message);

            // really want to batch process messages and call the below after processing a batch, or something equivalent
            if (messagesInMessageRecordQueue) await _messageProcessor.ProcessMessageRecordsFromQueue();
        }
    }
}
