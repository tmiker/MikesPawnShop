using MassTransit;
using Products.Shared.Messages;

namespace Products.Read.API.MessageConsumers
{
    public class ProductAddedConsumer : IConsumer<ProductAddedMessage>
    {
        private readonly ILogger<ProductAddedConsumer> _logger;

        public ProductAddedConsumer(ILogger<ProductAddedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductAddedMessage> context)
        {
            var message = context.Message;
            await Task.Run(() => _logger.LogInformation("Product Added: AggregateId = {message.AggregateId}, Name = {message.Name}", message.AggregateId, message.Name));
        }
    }
}
