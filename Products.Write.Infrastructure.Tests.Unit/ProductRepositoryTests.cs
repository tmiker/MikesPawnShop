using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Products.Write.Domain.Aggregates;
using Products.Write.Domain.Base;
using Products.Write.Domain.Enumerations;
using Products.Write.Domain.Events;
using Products.Write.Domain.Snapshots;
using Products.Write.Infrastructure.Abstractions;
using Products.Write.Infrastructure.Repositories;

namespace Products.Write.Infrastructure
{
    public class ProductRepositoryTests
    {
        //private readonly NullLogger<ProductRepository> _logger = NullLogger<ProductRepository>.Instance;
        //private readonly Mock<IProductEventStore> eventStoreMock = new Mock<IProductEventStore>();
        //private readonly Mock<IEventAggregator> eventAggregatorMock = new Mock<IEventAggregator>();
        //private readonly Mock<IProductEventHandlers> productEventHandlersMock = new Mock<IProductEventHandlers>();

        [Fact]
        public async Task GetProductByIdAsync_CallsEventStoreOnce_ReturnsProductInCorrectState()
        {
            // ARRANGE
            Guid aggregateId = Guid.NewGuid();
            string productName = "Book Name";
            string imageName = "Image Name";
            string docName = "Doc Name";
            string category = "Books";
            string status = "InActive";

            // Create the IEnumerable<IDomainEvent> returned by the Mock EventStore using Setup/Returns
            IDomainEvent productAdded = new ProductAdded(aggregateId, "Product", 0, "Correlation Id 1", productName,
                CategoryEnum.Books, "Book Description", 25.99m, "USD", Status.Active.Name, 1, 0, "each", 1);
            IDomainEvent statusUpdated = new StatusUpdated(aggregateId, "Product", 1, "Correlation Id 2", Status.InActive.Name);
            IDomainEvent imageAdded = new ImageAdded(aggregateId, "Product", 2, "Correlation Id 3", imageName, "Caption",
                4, "Image URL", "Thumb URL");
            IDomainEvent docAdded = new DocumentAdded(aggregateId, "Product", 3, "Correlation Id 4", docName, "Title",
                3, "Doc URL");
            IEnumerable<IDomainEvent> expectedEventsFromStore = new List<IDomainEvent>() { productAdded, statusUpdated, imageAdded, docAdded };

            // Set up dependencies
            Mock<IProductEventStore> eventStoreMock = new Mock<IProductEventStore>();
            eventStoreMock.Setup(x => x.GetDomainEventsByIdAsync(aggregateId, 0, int.MaxValue)).ReturnsAsync(expectedEventsFromStore);
            NullLogger<ProductRepository> _logger = NullLogger<ProductRepository>.Instance;

            // Instantiate SUT
            IProductRepository sut = new ProductRepository(eventStoreMock.Object, _logger);

            // ACT
            Product product = await sut.GetProductByIdAsync(aggregateId);
            // Get snapshot to expose encapsulated properties
            ProductSnapshot snapshot = product.GetSnapshot();

            // ASSERT
            // verify repository calls the appropriate event store method once
            eventStoreMock.Verify(x => x.GetDomainEventsByIdAsync(It.IsAny<Guid>(), 0, int.MaxValue), Times.Once);
            // verify repository reconstitutes the Product entity correctly
            Assert.Equal(product.Id, aggregateId);
            Assert.Equal(snapshot.Name, productName);
            Assert.Equal(snapshot.Category, category);
            Assert.Equal(snapshot.Status, status);
            Assert.Equal(snapshot.Images?.Count, 1);
            Assert.Equal(snapshot.Documents?.Count, 1);
            Assert.Equal(snapshot.Images?[0].Name, imageName);
            Assert.Equal(snapshot.Documents?[0].Name, docName);
        }

        [Fact]
        public async Task SaveAsync_ValidProjectWithMultipleEventsAsArgument_CallsEventStoreSaveMethodForEvents()
        {
            // ARRANGE
            // create a product with multiple domain events to save and a SUT ProjectRepository
            Product product = new Product("Product 1", CategoryEnum.Books, "A book on things.", 25.99m,
                "USD", "Active", 1, "each", 1, Guid.NewGuid().ToString());
            product.UpdateStatus("InActive", Guid.NewGuid().ToString());
            product.AddImage("Image 1", "A dog", "Image URL", "Thumb URL", Guid.NewGuid().ToString());
            product.AddDocument("Doc 1", "Instructions", "Document URL", Guid.NewGuid().ToString());

            ProductSnapshot snapshot = product.GetSnapshot();

            // Set up dependencies
            Mock<IProductEventStore> eventStoreMock = new Mock<IProductEventStore>();
            // eventStoreMock.Setup(x => x.GetDomainEventsByIdAsync(aggregateId, 0, int.MaxValue)).ReturnsAsync(expectedEventsFromStore);
            NullLogger<ProductRepository> _logger = NullLogger<ProductRepository>.Instance;

            // Instantiate SUT
            IProductRepository sut = new ProductRepository(eventStoreMock.Object, _logger);

            // ACT
            // call SaveAsync 
            await sut.SaveAsync(product);

            // ASSERT
            // verify eventstore dependency method SaveEventRecordsAsync() is called once for all events
            eventStoreMock.Verify(x => x.SaveEventRecordsAsync(It.IsAny<IEnumerable<IDomainEvent>>()), Times.Exactly(1));
        }
    }
}
