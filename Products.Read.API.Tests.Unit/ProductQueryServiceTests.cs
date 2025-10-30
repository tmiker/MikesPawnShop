using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Products.Read.API.Domain.Models;
using Products.Read.API.DTOs;
using Products.Read.API.Infrastructure.Data;
using Products.Read.API.QueryResponses;
using Products.Read.API.QueryServices;
using Products.Shared.Messages;

namespace Products.Read.API
{
    public class ProductQueryServiceTests
    {
        [Fact]
        public async Task GetAllProductSummariesAsync_IncludeImagesAndDocuments_ReturnsCorrectResult()
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
            int imageSequenceNumber = 1;
            string imageUrl = "https://www.docs.imageUrl";
            string thumbUrl = "https://www.docs.thumbUrl";
            int imageVersion = 1;
            string documentName = "Instructions";
            string title = "Meade LX8 Instructions";
            int documentSequenceNumber = 1;
            string documentUrl = "https://www.docs.documentUrl";
            int documentVersion = 1;

            NullLogger<ProductQueryService> logger = NullLogger<ProductQueryService>.Instance;
            var dbContextOptions = new DbContextOptionsBuilder<ProductsReadDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            IEnumerable<ProductSummaryDTO>? summaryDTOs = null;

            // Act
            using (var context = new ProductsReadDbContext(dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Product product = new Product(aggregateId, productName, category, description, price, currency, status, aggregateVersion);
                ImageData image = new ImageData(imageName, caption, imageSequenceNumber, imageUrl, thumbUrl);
                DocumentData document = new DocumentData(documentName, title, documentSequenceNumber, documentUrl);
                context.Products.Add(product);
                product.AddImage(image, imageVersion);
                product.AddDocument(document, documentVersion);
                context.SaveChanges();

                // int productId = product.Id;

                ProductQueryService queryService = new ProductQueryService(context, logger);

                GetProductSummariesResult result = await queryService.GetAllProductSummariesAsync();
                summaryDTOs = result.ProductSummaries;

            }

            // Assert
            using (var context = new ProductsReadDbContext(dbContextOptions))
            {
                Assert.NotNull(summaryDTOs);
                Assert.Single(summaryDTOs);
                Assert.All(summaryDTOs, s => Assert.Equal(1, s.ImageCount));
                Assert.All(summaryDTOs, s => Assert.Equal(1, s.DocumentCount));
            }
        }

        [Fact]
        public async Task GetPagedAndFilteredProductSummariesAsync_IncludeImagesAndDocuments_ReturnsCorrectResult()
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
            int imageSequenceNumber = 1;
            string imageUrl = "https://www.docs.imageUrl";
            string thumbUrl = "https://www.docs.thumbUrl";
            int imageVersion = 1;
            string documentName = "Instructions";
            string title = "Meade LX8 Instructions";
            int documentSequenceNumber = 1;
            string documentUrl = "https://www.docs.documentUrl";
            int documentVersion = 1;

            NullLogger<ProductQueryService> logger = NullLogger<ProductQueryService>.Instance;
            var dbContextOptions = new DbContextOptionsBuilder<ProductsReadDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            GetPagedAndFilteredProductSummariesResult? pagedAndFilteredSummariesResult = null;

            // Act
            using (var context = new ProductsReadDbContext(dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Product product = new Product(aggregateId, productName, category, description, price, currency, status, aggregateVersion);
                ImageData image = new ImageData(imageName, caption, imageSequenceNumber, imageUrl, thumbUrl);
                DocumentData document = new DocumentData(documentName, title, documentSequenceNumber, documentUrl);
                context.Products.Add(product);
                product.AddImage(image, imageVersion);
                product.AddDocument(document, documentVersion);
                context.SaveChanges();

                ProductQueryService queryService = new ProductQueryService(context, logger);

                pagedAndFilteredSummariesResult = await queryService.GetPagedAndFilteredProductSummariesAsync("mEAdE", "Astronomy", "Id", 1, 5);
            }

            // Assert
            using (var context = new ProductsReadDbContext(dbContextOptions))
            {
                Assert.NotNull(pagedAndFilteredSummariesResult.ProductSummaries);
                Assert.NotNull(pagedAndFilteredSummariesResult.PaginationMetadata);
                Assert.Single(pagedAndFilteredSummariesResult.ProductSummaries);
                Assert.Equal(1, pagedAndFilteredSummariesResult.PaginationMetadata.TotalItemCount);
                Assert.Equal(5, pagedAndFilteredSummariesResult.PaginationMetadata.PageSize);
                Assert.All(pagedAndFilteredSummariesResult.ProductSummaries, s => Assert.Equal(1, s.ImageCount));
                Assert.All(pagedAndFilteredSummariesResult.ProductSummaries, s => Assert.Equal(1, s.DocumentCount));
            }
        }
    }
}
