using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Products.Read.API.Domain.Models;
using Products.Read.API.Exceptions;
using Products.Read.API.Infrastructure.Data;
using Products.Read.API.Infrastructure.Repositories;
using Products.Read.API.Middleware;
using Products.Shared.Messages;
using System.Net;

namespace Products.Read.API
{
    public class ProductRepositoryTests
    {
        [Theory]
        [MemberData(nameof(Products.Read.API.ProductRepositoryMemberData.AddProductValidCommandTestData), MemberType = typeof(Products.Read.API.ProductRepositoryMemberData))]
        public async Task AddProductAsync_ValidInputArgument_AddsProduct(ProductAddedMessage productAddedMessage)
        {
            // Arrange
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
                Assert.Equal(productAddedMessage.AggregateVersion, product.Version);
                Assert.Equal(productAddedMessage.AggregateType, product.GetType().Name);
                Assert.Equal(productAddedMessage.AggregateId, product.AggregateId);
                Assert.Equal(productAddedMessage.Name, product.Name);
                Assert.Equal(productAddedMessage.Status, product.Status);
            }
        }

        [Theory]
        [MemberData(nameof(ProductRepositoryMemberData.AddProductInvalidNameInCommandTestData), MemberType = typeof(ProductRepositoryMemberData))]
        public async Task AddProductAsync_InValidProductNameArgument_ThrowsDataConsistencyExceptionException(ProductAddedMessage productAddedMessage)
        {
            // Arrange
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

        [Theory]
        [MemberData(nameof(ProductRepositoryMemberData.AddProductAndUpdateStatusValidCommandsTestData), MemberType = typeof(ProductRepositoryMemberData))]
        public async Task UpdateProductStatusAsync_ValidInputArgument_CorrectlyUpdatesProductStatus(ProductAddedMessage productAddedMessage, StatusUpdatedMessage statusUpdatedMessage)
        {
            // Arrange
            NullLogger<ProductRepository> logger = NullLogger<ProductRepository>.Instance;
            var dbContextOptions = new DbContextOptionsBuilder<ProductsReadDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using (var context = new ProductsReadDbContext(dbContextOptions))
            {
                // Act
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                ProductRepository productRepository = new ProductRepository(context, logger);

                await productRepository.AddProductAsync(productAddedMessage);
                await productRepository.UpdateProductStatusAsync(statusUpdatedMessage);

                // Assert
                Product product = context.Products.Single();
                Assert.NotNull(product);
                Assert.Equal(statusUpdatedMessage.AggregateVersion, product.Version);
                Assert.Equal(statusUpdatedMessage.Status, product.Status);
            }
        }

        [Theory]
        [MemberData(nameof(ProductRepositoryMemberData.AddProductAndUpdateStatusDuplicateProductVersionTestData), MemberType = typeof(ProductRepositoryMemberData))]
        public async Task UpdateProductStatusAsync_DuplicateProductMessage_IgnoresDuplicateMessage(ProductAddedMessage productAddedMessage, StatusUpdatedMessage firstStatusUpdatedMessage, StatusUpdatedMessage secondStatusUpdatedMessage)
        {
            // Arrange
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
                Assert.Equal(firstStatusUpdatedMessage.AggregateVersion, product.Version);
                Assert.Equal(firstStatusUpdatedMessage.Status, product.Status);
            }
        }

        [Theory]
        [MemberData(nameof(ProductRepositoryMemberData.AddProductAndAddImageCommandsTestData), MemberType = typeof(ProductRepositoryMemberData))]
        public async Task AddProductImageAsync_ValidInputArgument_AddsImageToProduct(ProductAddedMessage productAddedMessage, ImageAddedMessage imageAddedMessage)
        {
            // Arrange
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
                Assert.Equal(imageAddedMessage.Caption, image.Caption);
            }
        }

        [Theory]
        [MemberData(nameof(ProductRepositoryMemberData.AddProductAndAddDocumentCommandsTestData), MemberType = typeof(ProductRepositoryMemberData))]
        public async Task AddProductDocumentAsync_ValidInputArgument_AddsDocumentToProduct(ProductAddedMessage productAddedMessage, DocumentAddedMessage documentAddedMessage)
        {
            // Arrange
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
                Assert.Equal(documentAddedMessage.Title, document.Title);
            }
        }


        //// DUE TO REFACTORING REPOSITORY TO USE RETRIES, NEED TO REFACTOR THIS TEST TO USE A STUB THAT ALLOWS MULTIPLE CALLS TO REPOSITORY
        //[Theory]
        //[MemberData(nameof(ProductRepositoryMemberData.AddProductAndUpdateStatusMissingProductVersionTestData), MemberType = typeof(ProductRepositoryMemberData))]
        //public async Task UpdateProductStatusAsync_MissingProductVersion_ThrowsDataConsistencyException(ProductAddedMessage productAddedMessage, StatusUpdatedMessage statusUpdatedMessage)
        //{
        //    // Arrange
        //    NullLogger<ProductRepository> logger = NullLogger<ProductRepository>.Instance;
        //    var dbContextOptions = new DbContextOptionsBuilder<ProductsReadDbContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        //    using (var context = new ProductsReadDbContext(dbContextOptions))
        //    {
        //        // Act
        //        context.Database.EnsureDeleted();
        //        context.Database.EnsureCreated();

        //        ProductRepository productRepository = new ProductRepository(context, logger);

        //        await productRepository.AddProductAsync(productAddedMessage);

        //        // Assert
        //        Product product = context.Products.Single();
        //        Assert.NotNull(product);
        //        Assert.Equal(productAddedMessage.AggregateVersion, product.Version);
        //        Assert.Equal(productAddedMessage.Status, product.Status);

        //        // *** the below call will drive the repository method GetCorrectProductAndVersionWithRetriesAsync to make multiple attempts to get the correct version
        //        await Assert.ThrowsAsync<DataConsistencyException>(async () => await productRepository.UpdateProductStatusAsync(statusUpdatedMessage));
        //    }
        //}

        //// DUE TO REFACTORING REPOSITORY TO USE RETRIES, NEED TO REFACTOR THIS TEST TO USE A STUB THAT ALLOWS MULTIPLE CALLS TO REPOSITORY
        //[Theory]
        //[MemberData(nameof(ProductRepositoryMemberData.AddProductAndUpdateStatusProductNotFoundTestData), MemberType = typeof(ProductRepositoryMemberData))]
        //public async Task UpdateProductStatusAsync_ProductNotFound_ThrowsDataConsistencyException(ProductAddedMessage productAddedMessage, StatusUpdatedMessage statusUpdatedMessage)
        //{
        //    // Arrange
        //    NullLogger<ProductRepository> logger = NullLogger<ProductRepository>.Instance;
        //    var dbContextOptions = new DbContextOptionsBuilder<ProductsReadDbContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        //    // Act
        //    using (var context = new ProductsReadDbContext(dbContextOptions))
        //    {
        //        context.Database.EnsureDeleted();
        //        context.Database.EnsureCreated();

        //        ProductRepository productRepository = new ProductRepository(context, logger);
        //        await productRepository.AddProductAsync(productAddedMessage);

        //        // Assert
        //        // *** the below call will drive the repository method GetCorrectProductAndVersionWithRetriesAsync to make multiple attempts to get a product with correct aggregateId
        //        await Assert.ThrowsAnyAsync<Exception>(async () => await productRepository.UpdateProductStatusAsync(statusUpdatedMessage));
        //    }
        //}
    }
}
