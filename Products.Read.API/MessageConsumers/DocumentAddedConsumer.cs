using MassTransit;
using Products.Read.API.Abstractions;
using Products.Shared.Messages;

namespace Products.Read.API.MessageConsumers
{
    public class DocumentAddedConsumer : IConsumer<DocumentAddedMessage>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<DocumentAddedConsumer> _logger;

        public DocumentAddedConsumer(IProductRepository productRepository, ILogger<DocumentAddedConsumer> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DocumentAddedMessage> context)
        {
            var message = context.Message;
            await Task.Run(() => _logger.LogInformation("Document Added Message Received: VERSION = {version}, AggregateId = {message.AggregateId}, " +
                "Title = {message.Title}", message.AggregateVersion, message.AggregateId, message.Title));

            await _productRepository.AddProductDocumentAsync(message);
        }
    }
}
