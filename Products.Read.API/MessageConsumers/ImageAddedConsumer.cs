using MassTransit;
using Products.Read.API.Abstractions;
using Products.Shared.Messages;

namespace Products.Read.API.MessageConsumers
{
    public class ImageAddedConsumer : IConsumer<ImageAddedMessage>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ImageAddedConsumer> _logger;

        public ImageAddedConsumer(IProductRepository productRepository, ILogger<ImageAddedConsumer> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ImageAddedMessage> context)
        {
            var message = context.Message;
            _logger.LogInformation("Image Added Message Received: VERSION = {version}, AggregateId = {message.AggregateId}, " +
                "Caption = {message.Caption}", message.AggregateVersion, message.AggregateId, message.Caption);

            await _productRepository.AddProductImageAsync(message);
        }
    }
}
