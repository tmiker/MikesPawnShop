using MassTransit;
using Products.Read.API.Abstractions;
using Products.Shared.Messages;

namespace Products.Read.API.MessageConsumers
{
    public class DataPurgedConsumer : IConsumer<DataPurgedMessage>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<DataPurgedConsumer> _logger;

        public DataPurgedConsumer(IProductRepository productRepository, ILogger<DataPurgedConsumer> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<DataPurgedMessage> context)
        {
            var message = context.Message;
            _logger.LogInformation("Data Purged Message Received");
            
            bool success = await _productRepository.PurgeAsync();
            _logger.LogInformation("Data successfully purged.");
        }
    }
}
