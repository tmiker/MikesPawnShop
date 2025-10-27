using Microsoft.Extensions.Logging;
using Products.Write.Application.Abstractions;
using Products.Write.Application.CQRS.CommandResults;
using System.Runtime.CompilerServices;

namespace Products.Write.Application.Services
{
    public class ProductCommandManagementService : IProductCommandManagementService
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogger<ProductCommandManagementService> _logger;

        public ProductCommandManagementService(ICommandDispatcher commandDispatcher, IEventAggregator eventAggregator, ILogger<ProductCommandManagementService> logger)
        {
            _commandDispatcher = commandDispatcher;
            _eventAggregator = eventAggregator;
            _logger = logger;
        }

        public async Task<TResult> ExecuteCommandAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Executing command {CommandType}", typeof(TCommand).Name);
            TResult result = await _commandDispatcher.DispatchAsync<TCommand, TResult>(command, cancellationToken);
            return result;
        }
    }
}
