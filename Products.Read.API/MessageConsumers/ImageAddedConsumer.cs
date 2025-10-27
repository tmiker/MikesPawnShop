using MassTransit;
using Products.Shared.Messages;

namespace Products.Read.API.MessageConsumers
{
    public class ImageAddedConsumer : IConsumer<ImageAddedMessage>
    {
        public async Task Consume(ConsumeContext<ImageAddedMessage> context)
        {

        }
    }
}
