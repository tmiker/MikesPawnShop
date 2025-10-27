using MassTransit;
using Products.Shared.Messages;

namespace Products.Read.API.MessageConsumers
{
    public class ProductAddedConsumer : IConsumer<ProductAddedMessage>
    {
        public async Task Consume(ConsumeContext<ProductAddedMessage> context)
        {

        }
    }
}
