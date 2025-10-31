using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Products.Shared.Messages;
using Products.Write.Application.Abstractions;
using Products.Write.Application.CQRS.CommandResults;
using Products.Write.Application.CQRS.Commands;
using Products.Write.Application.CQRS.DevTests;
using Products.Write.Application.DTOs;
using Products.Write.Domain.Enumerations;
using Products.Write.Infrastructure.Abstractions;

namespace Products.Write.Application.CQRS.CommandHandlers
{
    public class PurgeDataHandler : ICommandHandler<PurgeData, PurgeDataResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PurgeDataHandler> _logger;

        public PurgeDataHandler(IProductRepository productRepository, IPublishEndpoint publishEndpoint, IConfiguration configuraiton, ILogger<PurgeDataHandler> logger)
        {
            _productRepository = productRepository;
            _publishEndpoint = publishEndpoint;
            _configuration = configuraiton;
            _logger = logger;
        }

        public async Task<PurgeDataResult> HandleAsync(PurgeData command, CancellationToken cancellationToken)
        {
            string? pinString = _configuration["PurgeDataPinNumber"] ?? throw new InvalidOperationException("Pin Number is null");
            int pin = Int32.Parse(pinString);

            if (command.PinNumber != pin) throw new InvalidOperationException("Pin Number is invalid");

            if (command.CorrelationId is null) command.CorrelationId = Guid.NewGuid().ToString();

            bool success = await _productRepository.PurgeAsync();

            if (success)
            {
                _logger.LogInformation("Data successfully purged.");

                DataPurgedMessage purgedMessage = new DataPurgedMessage();

                await _publishEndpoint.Publish(purgedMessage);

                return new PurgeDataResult(true, null);
            }
            else return new PurgeDataResult(false, "An error occurred while purging data.");
        }
    }
}
