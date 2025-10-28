using MassTransit;
using Products.Shared.Messages;

namespace Products.Read.API.MessageConsumers
{
    public class DocumentAddedConsumer : IConsumer<DocumentAddedMessage>
    {
        private readonly ILogger<DocumentAddedConsumer> _logger;

        public DocumentAddedConsumer(ILogger<DocumentAddedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DocumentAddedMessage> context)
        {
            var message = context.Message;
            await Task.Run(() => _logger.LogInformation("Document Added: AggregateId = {message.AggregateId}, Title = {message.Title}", message.AggregateId, message.Title));
        }
    }
}
