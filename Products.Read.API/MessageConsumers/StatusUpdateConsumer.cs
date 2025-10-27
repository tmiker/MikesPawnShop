using MassTransit;
using Products.Shared.Messages;

namespace Products.Read.API.MessageConsumers
{
    public class StatusUpdateConsumer : IConsumer<StatusUpdatedMessage>
    {
        public async Task Consume(ConsumeContext<StatusUpdatedMessage> context)
        {
        }
    }
}
