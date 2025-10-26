using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Logging;
using Products.Write.Application.Abstractions;
using Products.Write.Application.CQRS.CommandHandlers;
using Products.Write.Application.CQRS.CommandResults;
using Products.Write.Application.CQRS.Commands;
using Products.Write.Application.Exceptions;
using Products.Write.Domain.Enumerations;
using Products.Write.Infrastructure.Abstractions;

namespace Products.Write.Application.CQRS.DevTests
{
    public class ThrowExceptionHandler : ICommandHandler<ThrowException, ThrowExceptionResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogger<ThrowExceptionHandler> _logger;

        public ThrowExceptionHandler(IProductRepository productRepository, IEventAggregator eventAggregator, ILogger<ThrowExceptionHandler> logger)
        {
            _productRepository = productRepository;
            _eventAggregator = eventAggregator;
            _logger = logger;
        }

        public async Task<ThrowExceptionResult> HandleAsync(ThrowException command, CancellationToken cancellationToken)
        {
            if (command.CorrelationId is null) command.CorrelationId = Guid.NewGuid().ToString();

            await Task.Run(() => _logger.LogInformation("ThrowExceptionHandler will now throw an exception of type {exceptionType} " +
                "with CorrelationId: {correlationId}", command.ExceptionType, command.CorrelationId));

            Exception ex = command.ExceptionType.ToLower() switch
            {
                "validationexception" => throw new ValidationException("This is a test ValidationException thrown from ThrowExceptionHandler."),
                "unauthorizedaccessexception" => throw new UnauthorizedAccessException("This is a test UnauthorizedAccessException thrown from ThrowExceptionHandler."),
                "forbiddenexception" => throw new ForbiddenException("This is a test ForbiddenException thrown from ThrowExceptionHandler."),
                "notfoundexception" => throw new NotFoundException("This is a test NotFoundException thrown from ThrowExceptionHandler."),
                "conflictexception" => throw new ConflictException("This is a test ConflictException thrown from ThrowExceptionHandler."),
                "argumentexception" => throw new ArgumentException("This is a test ArgumentException thrown from ThrowExceptionHandler."),
                "argumentnullexception" => throw new ArgumentNullException("This is a test ArgumentNullException thrown from ThrowExceptionHandler."),
                "invalidoperationexception" => throw new InvalidOperationException("This is a test InvalidOperationException thrown from ThrowExceptionHandler."),
                "taskcanceledException" => throw new TaskCanceledException("This is a test TaskCanceledException thrown from ThrowExceptionHandler."),
                _ => throw new Exception("This is a test general Exception thrown from ThrowExceptionHandler.")
            };

            return new ThrowExceptionResult(false, "An exception should have been thrown, so something went wrong.");
        }
    }
}
