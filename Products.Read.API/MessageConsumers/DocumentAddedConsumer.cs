using MassTransit;
using Products.Shared.Messages;

namespace Products.Read.API.MessageConsumers
{
    public class DocumentAddedConsumer : IConsumer<DocumentAddedMessage>
    {
        public async Task Consume(ConsumeContext<DocumentAddedMessage> context)
        {
            // Implementation for handling the DocumentAddedMessage
        }
    }
}
