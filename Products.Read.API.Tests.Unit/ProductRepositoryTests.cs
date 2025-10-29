using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Products.Read.API.Domain.Models;
using Products.Read.API.Infrastructure.Data;
using Products.Read.API.Infrastructure.Repositories;
using Products.Shared.Messages;

namespace Products.Read.API
{
    public class ProductRepositoryTests
    {
        [Fact]
        public async Task AddProductAsync_ValidInputArgument_AddsProduct()
        {

        }

        [Fact]
        public async Task AddProductDocumentAsync_ValidInputArgument_AddsDocumentToProduct()
        {
            // Arrange
            Guid aggregateId = Guid.NewGuid();
            string aggregateType = "Product";
            int aggregateVersion = 0;
            string correlationId = Guid.NewGuid().ToString();
            string productName = "Meade LX8";
            string category = "Astronomy";
            string description = "Catadioptric Telescope";
            decimal price = 1299.99m;
            string currency = "USD";
            string status = "Active";
            string documentName = "Instructions";
            string title = "Meade LX8 Instructions";
            int sequenceNumber = 1;
            string documentUrl = "https://www.docs.documentUrl";

            ProductAddedMessage productAddedMessage = new ProductAddedMessage(aggregateId, aggregateType, aggregateVersion,
                correlationId, productName, category, description, price, currency, status);
            DocumentAddedMessage documentAddedMessage = new DocumentAddedMessage(aggregateId, aggregateType, aggregateVersion,
                correlationId, documentName, title, sequenceNumber, documentUrl);

            NullLogger<ProductRepository> logger = NullLogger<ProductRepository>.Instance;
            var dbContextOptions = new DbContextOptionsBuilder<ProductsReadDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            // Act
            using (var context = new ProductsReadDbContext(dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                ProductRepository productRepository = new ProductRepository(context, logger);

                await productRepository.AddProductAsync(productAddedMessage);
                await productRepository.AddProductDocumentAsync(documentAddedMessage);
            }

            // Assert
            using (var context = new ProductsReadDbContext(dbContextOptions))
            {
                Product product = context.Products.Include(p => p.Documents).Single();
                DocumentData document = product.Documents?[0]!;
                Assert.NotNull(product);
                Assert.NotNull(product.Documents);
                Assert.Single(product.Documents);
                Assert.Equal(document.ProductId, product.Id);
                Assert.Equal(document.Title, title);
            }
        }
    }
}
