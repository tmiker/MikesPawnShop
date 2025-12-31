using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Products.Read.API.Domain.Models;
using Products.Read.API.DTOs;
using Products.Read.API.Infrastructure.Data;
using Products.Read.API.QueryResponses;
using Products.Read.API.QueryServices;

namespace Products.Read.API
{
    public class ProductQueryServiceTests
    {
        [Theory]
        [ClassData(typeof(ProductQueryServiceTestsClassData))]
        public async Task GetAllProductSummariesAsync_IncludeImagesAndDocuments_ReturnsCorrectResult(Product product, ImageData image, DocumentData document)
        {
            // Arrange
            int imageVersion = 1;   // should be aggregateVersion + 1
            int documentVersion = 2;    // should be aggregateVersion + 2

            NullLogger<ProductQueryService> logger = NullLogger<ProductQueryService>.Instance;
            var dbContextOptions = new DbContextOptionsBuilder<ProductsReadDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            IEnumerable<ProductSummaryDTO>? summaryDTOs = null;

            // Act
            using (var context = new ProductsReadDbContext(dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Products.Add(product);
                product.AddImage(image, imageVersion);
                product.AddDocument(document, documentVersion);
                context.SaveChanges();

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

        [Theory]
        [ClassData(typeof(ProductQueryServiceTestsClassData))]
        public async Task GetPagedAndFilteredProductSummariesAsync_IncludeImagesAndDocuments_ReturnsCorrectResult(Product product, ImageData image, DocumentData document)
        {
            // Arrange
            int imageVersion = 1;   // should be aggregateVersion + 1
            int documentVersion = 2;    // should be aggregateVersion + 2

            NullLogger<ProductQueryService> logger = NullLogger<ProductQueryService>.Instance;
            var dbContextOptions = new DbContextOptionsBuilder<ProductsReadDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            GetPagedAndFilteredProductSummariesResult? pagedAndFilteredSummariesResult = null;

            // Act
            using (var context = new ProductsReadDbContext(dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

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
