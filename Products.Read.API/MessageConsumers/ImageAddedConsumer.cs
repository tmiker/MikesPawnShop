using MassTransit;
using Products.Shared.Messages;

namespace Products.Read.API.MessageConsumers
{
    public class ImageAddedConsumer : IConsumer<ImageAddedMessage>
    {
        private readonly ILogger<ImageAddedConsumer> _logger;

        public ImageAddedConsumer(ILogger<ImageAddedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ImageAddedMessage> context)
        {
            var message = context.Message;
            await Task.Run(() => _logger.LogInformation("Image Added: AggregateId = {message.AggregateId}, Caption = {message.Caption}", message.AggregateId, message.Caption));
        }
    }
}
