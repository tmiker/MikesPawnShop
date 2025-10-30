using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Products.Read.API.Domain.Models;
using Products.Read.API.Exceptions;
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

            ProductAddedMessage productAddedMessage = new ProductAddedMessage(aggregateId, aggregateType, aggregateVersion,
                correlationId, productName, category, description, price, currency, status);

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
            }

            // Assert
            using (var context = new ProductsReadDbContext(dbContextOptions))
            {
                Product product = context.Products.Single();
                Assert.NotNull(product);
                Assert.Equal(aggregateVersion, product.Version);
                Assert.Equal(aggregateType, product.GetType().Name);
                Assert.Equal(aggregateId, product.AggregateId);
                Assert.Equal(productName , product.Name);
                Assert.Equal(status, product.Status);
            }
        }

        [Fact]
        public async Task AddProductAsync_InValidProductNameArgument_ThrowsDataConsistencyExceptionException()
        {
            Guid aggregateId = Guid.NewGuid();
            string aggregateType = "Product";
            int aggregateVersion = 0;
            string correlationId = Guid.NewGuid().ToString();
            string productName = null!;
            string category = "Astronomy";
            string description = "Catadioptric Telescope";
            decimal price = 1299.99m;
            string currency = "USD";
            string status = "Active";

            ProductAddedMessage productAddedMessage = new ProductAddedMessage(aggregateId, aggregateType, aggregateVersion,
                correlationId, productName, category, description, price, currency, status);

            NullLogger<ProductRepository> logger = NullLogger<ProductRepository>.Instance;

            var dbContextOptions = new DbContextOptionsBuilder<ProductsReadDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            // Act
            using (var context = new ProductsReadDbContext(dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                ProductRepository productRepository = new ProductRepository(context, logger);

                // Assert
                await Assert.ThrowsAsync<DataConsistencyException>(async () => await productRepository.AddProductAsync(productAddedMessage));
            }
        }

        [Fact]
        public async Task UpdateProductStatusAsync_ValidInputArgument_CorrectlyUpdatesProductStatus()
        {
            // Arrange
            Guid aggregateId = Guid.NewGuid();
            string aggregateType = "Product";
            int initialVersion = 0;
            string correlationId = Guid.NewGuid().ToString();
            string productName = "Meade LX8";
            string category = "Astronomy";
            string description = "Catadioptric Telescope";
            decimal price = 1299.99m;
            string currency = "USD";
            string initialStatus = "Active";
            int updatedVersion = 1;
            string updatedStatus = "InActive";

            ProductAddedMessage productAddedMessage = new ProductAddedMessage(aggregateId, aggregateType, initialVersion,
                correlationId, productName, category, description, price, currency, initialStatus);
            StatusUpdatedMessage statusUpdatedMessage = new StatusUpdatedMessage(aggregateId, aggregateType, updatedVersion,
                correlationId, updatedStatus);

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
                await productRepository.UpdateProductStatusAsync(statusUpdatedMessage);
            }

            // Assert
            using (var context = new ProductsReadDbContext(dbContextOptions))
            {
                Product product = context.Products.Single();
                Assert.NotNull(product);
                Assert.Equal(updatedVersion, product.Version);
                Assert.Equal(updatedStatus, product.Status);
            }
        }

        [Fact]
        public async Task UpdateProductStatusAsync_ProductNotFound_ThrowsDataConsistencyException()
        {
            // Arrange
            Guid aggregateId = Guid.NewGuid();
            string aggregateType = "Product";
            int initialVersion = 0;
            string correlationId = Guid.NewGuid().ToString();
            string productName = "Meade LX8";
            string category = "Astronomy";
            string description = "Catadioptric Telescope";
            decimal price = 1299.99m;
            string currency = "USD";
            string initialStatus = "Active";
            int updatedVersion = 1;
            string updatedStatus = "InActive";
            Guid incorrectAggregateId = Guid.NewGuid();

            ProductAddedMessage productAddedMessage = new ProductAddedMessage(aggregateId, aggregateType, initialVersion,
                correlationId, productName, category, description, price, currency, initialStatus);
            StatusUpdatedMessage statusUpdatedMessage = new StatusUpdatedMessage(incorrectAggregateId, aggregateType, updatedVersion,
                correlationId, updatedStatus);

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

                // Assert
                Product product = context.Products.Single();
                Assert.NotNull(product);
                await Assert.ThrowsAsync<DataConsistencyException>(async () => await productRepository.UpdateProductStatusAsync(statusUpdatedMessage));
            }
        }

        [Fact]
        public async Task UpdateProductStatusAsync_MissingProductVersion_ThrowsMissingProductVersionException()
        {
            // Arrange
            Guid aggregateId = Guid.NewGuid();
            string aggregateType = "Product";
            int initialVersion = 0;
            string correlationId = Guid.NewGuid().ToString();
            string productName = "Meade LX8";
            string category = "Astronomy";
            string description = "Catadioptric Telescope";
            decimal price = 1299.99m;
            string currency = "USD";
            string initialStatus = "Active";
            int updatedVersion = 2;                 // version 1 will be missing, should throw MissingProductVersionException
            string updatedStatus = "InActive";
            Guid incorrectAggregateId = Guid.NewGuid();

            ProductAddedMessage productAddedMessage = new ProductAddedMessage(aggregateId, aggregateType, initialVersion,
                correlationId, productName, category, description, price, currency, initialStatus);
            StatusUpdatedMessage statusUpdatedMessage = new StatusUpdatedMessage(incorrectAggregateId, aggregateType, updatedVersion,
                correlationId, updatedStatus);

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

                // Assert
                Product product = context.Products.Single();
                Assert.NotNull(product);
                await Assert.ThrowsAsync<DataConsistencyException>(async () => await productRepository.UpdateProductStatusAsync(statusUpdatedMessage));
            }
        }

        [Fact]
        public async Task UpdateProductStatusAsync_DuplicateProductMessage_IgnoresDuplicateMessage()
        {
            // Arrange
            Guid aggregateId = Guid.NewGuid();
            string aggregateType = "Product";
            int initialVersion = 3;
            string correlationId = Guid.NewGuid().ToString();
            string productName = "Meade LX8";
            string category = "Astronomy";
            string description = "Catadioptric Telescope";
            decimal price = 1299.99m;
            string currency = "USD";
            string initialStatus = "Active";
            int firstUpdatedVersion = 4;                 // version 1 will be missing, should throw MissingProductVersionException
            string firstUpdatedStatus = "InActive";
            int secondUpdatedVersion = 2;
            string secondUpdatedStatus = "Obsolete";

            ProductAddedMessage productAddedMessage = new ProductAddedMessage(aggregateId, aggregateType, initialVersion,
                correlationId, productName, category, description, price, currency, initialStatus);
            StatusUpdatedMessage firstStatusUpdatedMessage = new StatusUpdatedMessage(aggregateId, aggregateType, firstUpdatedVersion,
                correlationId, firstUpdatedStatus);
            StatusUpdatedMessage secondStatusUpdatedMessage = new StatusUpdatedMessage(aggregateId, aggregateType, secondUpdatedVersion,
                correlationId, secondUpdatedStatus);

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
                await productRepository.UpdateProductStatusAsync(firstStatusUpdatedMessage);
                await productRepository.UpdateProductStatusAsync(secondStatusUpdatedMessage);

                // Assert
                Product product = context.Products.Single();
                Assert.NotNull(product);
                Assert.Equal(firstUpdatedStatus, product.Status);
            }
        }

        [Fact]
        public async Task AddProductImageAsync_ValidInputArgument_AddsImageToProduct()
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
            string imageName = "Telescope";
            string caption = "Meade LX8";
            int sequenceNumber = 1;
            string imageUrl = "https://www.docs.imageUrl";
            string thumbUrl = "https://www.docs.thumbUrl";
            int imageVersion = 1;

            ProductAddedMessage productAddedMessage = new ProductAddedMessage(aggregateId, aggregateType, aggregateVersion,
                correlationId, productName, category, description, price, currency, status);
            ImageAddedMessage imageAddedMessage = new ImageAddedMessage(aggregateId, aggregateType, imageVersion,
                correlationId, imageName, caption, sequenceNumber, imageUrl, thumbUrl);

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
                await productRepository.AddProductImageAsync(imageAddedMessage);
            }

            // Assert
            using (var context = new ProductsReadDbContext(dbContextOptions))
            {
                Product product = context.Products.Include(p => p.Images).Single();
                ImageData image = product.Images?[0]!;
                Assert.NotNull(product);
                Assert.NotNull(product.Images);
                Assert.Single(product.Images);
                Assert.Equal(image.ProductId, product.Id);
                Assert.Equal(image.Caption, caption);
            }

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
            int documentVersion = 1;

            ProductAddedMessage productAddedMessage = new ProductAddedMessage(aggregateId, aggregateType, aggregateVersion,
                correlationId, productName, category, description, price, currency, status);
            DocumentAddedMessage documentAddedMessage = new DocumentAddedMessage(aggregateId, aggregateType, documentVersion,
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


        //[Theory]
        //[MemberData(nameof(ProductRepositoryMemberData.AddProductAndAddImageCommandsTestData), typeof(ProductRepositoryTests))]
        //public async Task AddProductImageAsync_ValidInputArgument_AddsImageToProduct_MemberData(ProductAddedMessage productAdded, ImageAddedMessage imageAdded)
        //{

        //}

    }
}
