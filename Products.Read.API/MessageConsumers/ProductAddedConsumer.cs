using MassTransit;
using Products.Read.API.Abstractions;
using Products.Shared.Messages;

namespace Products.Read.API.MessageConsumers
{
    public class ProductAddedConsumer : IConsumer<ProductAddedMessage>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductAddedConsumer> _logger;

        public ProductAddedConsumer(IProductRepository productRepository, ILogger<ProductAddedConsumer> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductAddedMessage> context)
        {
            var message = context.Message;
            await Task.Run(() => _logger.LogInformation("Product Added Message Received: VERSION = {version}, AggregateId = {message.AggregateId}," +
                " Name = {message.Name}", message.AggregateVersion, message.AggregateId, message.Name));

            await _productRepository.AddProductAsync(message);
        }
    }
}
