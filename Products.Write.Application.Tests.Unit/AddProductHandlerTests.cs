using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Products.Write.Application.Abstractions;
using Products.Write.Application.CQRS.CommandHandlers;
using Products.Write.Application.CQRS.Commands;
using Products.Write.Infrastructure.Abstractions;

namespace Products.Write.Application.Tests.Unit
{
    public class AddProductHandlerTests
    {


        [Fact]
        public async Task HandleAsync_ValidCommand_CallsRepositorySaveAndRaisesEvent()
        {
            // Arrange
            Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(repo => repo.SaveAsync(It.IsAny<Products.Write.Domain.Aggregates.Product>()))
                    .ReturnsAsync(true);

            Mock<IEventAggregator> eventAggregatorMock = new Mock<IEventAggregator>();
            // eventAggregatorMock.Setup(agg => agg.Raise(It.IsAny<Products.Write.Domain.Base.IDomainEvent>()));

            NullLogger<AddProductHandler> logger = NullLogger<AddProductHandler>.Instance;

            AddProductHandler _handler = new AddProductHandler(productRepositoryMock.Object, eventAggregatorMock.Object, logger);

            AddProduct addProductCommand = new AddProduct
            (
                "Test Product",                 // Name
                "Electronics",                  // Category
                "A test product description",   // Description
                99.99m,                         // Price
                "USD",                          // Currency
                "Active",                       // Status
                100,                            // QuantityOnHand
                "each",                         // UOM
                10,                             // LowStockThreshold
                Guid.NewGuid().ToString()       // CorrelationId
            );
            
            
            // Act
            var result = await _handler.HandleAsync(addProductCommand, CancellationToken.None);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.ProductId);
            Assert.Null(result.ErrorMessage);
            productRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<Products.Write.Domain.Aggregates.Product>()), Times.Once);
            eventAggregatorMock.Verify(agg => agg.Raise(It.IsAny<Products.Write.Domain.Events.ProductAdded>()), Times.AtLeastOnce);
        }
    }
}
