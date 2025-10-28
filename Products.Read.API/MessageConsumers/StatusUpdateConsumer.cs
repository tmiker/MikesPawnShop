using MassTransit;
using Products.Shared.Messages;

namespace Products.Read.API.MessageConsumers
{
    public class StatusUpdateConsumer : IConsumer<StatusUpdatedMessage>
    {
        private readonly ILogger<StatusUpdateConsumer> _logger;

        public StatusUpdateConsumer(ILogger<StatusUpdateConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<StatusUpdatedMessage> context)
        {
            var message = context.Message;
            await Task.Run(() => _logger.LogInformation("Status Updated: AggregateId = {message.AggregateId}, Status = {message.Status}", message.AggregateId, message.Status));
        }
    }
}
