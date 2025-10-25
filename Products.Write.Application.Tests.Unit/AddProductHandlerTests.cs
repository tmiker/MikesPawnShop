using Microsoft.Extensions.Logging;
using Moq;
using Products.Write.Application.CQRS.CommandHandlers;
using Products.Write.Infrastructure.Abstractions;

namespace Products.Write.Application.Tests.Unit
{
    public class AddProductHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IEventAggregator> _eventAggregatorMock;
        private readonly Mock<ILogger<AddProductHandler>> _loggerMock;
        private readonly AddProductHandler _handler;
    }
}
