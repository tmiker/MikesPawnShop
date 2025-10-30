﻿using Microsoft.EntityFrameworkCore;
using Products.Read.API.Abstractions;
using Products.Read.API.Domain.Models;
using Products.Read.API.DTOs;
using Products.Read.API.Infrastructure.Data;
using Products.Read.API.Paging;
using Products.Read.API.QueryResponses;
using static System.Net.Mime.MediaTypeNames;

namespace Products.Read.API.QueryServices
{
    /// <summary>
    /// ProductQueryService uses the EF DbContext directly versus implementing a repository for performance.
    /// State is not modified - queries only.
    /// </summary>
    public class ProductQueryService : IProductQueryService
    {
        private readonly ProductsReadDbContext _db;
        private readonly ILogger<ProductQueryService> _logger;

        public ProductQueryService(ProductsReadDbContext db, ILogger<ProductQueryService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IAsyncEnumerable<Product> GetProductsAsAsyncEnumerable()
        {
            return _db.Products.AsAsyncEnumerable();
        }

        public async Task<GetProductsResult> GetAllProductsAsync()
        {
            IEnumerable<Product> products = await _db.Products.Include(p => p.Images).Include(p => p.Documents).ToListAsync();
            List<ProductDTO> productDTOs = MapProductsToDTOs(products); 
            return new GetProductsResult(true, productDTOs, null);
        }

        public async Task<GetPagedAndFilteredProductsResult> GetPagedAndFilteredProductsAsync(
            string? filter, string? category, string? sortColumn, int pageNumber = 1, int pageSize = 10)
        {
            var query = _db.Products.Include(p => p.Images).Include(p => p.Documents).AsQueryable();

            if (!string.IsNullOrWhiteSpace(category)) query = query.Where(c => c.Category.ToLower() == category.ToLower());

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(p => p.Name.ToLower().Contains(filter.ToLower()) || p.Description.ToLower().Contains(filter.ToLower()));
            }

            switch (sortColumn?.ToLower())
            {
                case "id":
                    query = query.OrderBy(p => p.Id);
                    break;
                case "name":
                    query = query.OrderBy(p => p.Name);
                    break;
                case "price ascending":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "price descending":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                default:
                    query = query.OrderBy(p => p.Name);
                    break;
            }

            var products = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            List<ProductDTO> productDTOs = MapProductsToDTOs(products);
            PaginationMetadata metadata = new PaginationMetadata(products.Count(), pageSize, pageNumber);

            return new GetPagedAndFilteredProductsResult(true, productDTOs, metadata, null);
        }

        public async Task<GetProductByIdResult> GetProductByIdAsync(int id)
        {
            Product? product = await _db.Products.Include(p => p.Images).Include(p => p.Documents).FirstOrDefaultAsync(p => p.Id == id);
            if (product is null) return new GetProductByIdResult(false, null, $"A product with Id {id} was not found.");

            ProductDTO productDTO = new ProductDTO
            {
                Id = product.Id,
                AggregateId = product.AggregateId,
                Name = product.Name,
                Category = product.Category,
                Description = product.Description,
                Price = product.Price,
                Status = product.Status,
                Version = product.Version,
                DateCreated = product.DateCreated,
                DateUpdated = product.DateUpdated,
                Images = MapImagesToDTOs(product.Images!),
                Documents = MapDocumentsToDTOs(product.Documents!)
            };

            return new GetProductByIdResult(true, productDTO, null);
        }

        private List<ProductDTO> MapProductsToDTOs(IEnumerable<Product> products)
        {
            List<ProductDTO> productDTOs = new List<ProductDTO>();

            foreach (var product in products)
            {
                ProductDTO dto = new ProductDTO
                {
                    Id = product.Id,
                    AggregateId = product.AggregateId,
                    Name = product.Name,
                    Category = product.Category,
                    Description = product.Description,
                    Price = product.Price,
                    Status = product.Status,
                    Version = product.Version,
                    DateCreated = product.DateCreated,
                    DateUpdated = product.DateUpdated,
                    Images = MapImagesToDTOs(product.Images!),
                    Documents = MapDocumentsToDTOs(product.Documents!)
                };

                productDTOs.Add(dto);
            }

            return productDTOs;
        }

        private List<ImageDataDTO> MapImagesToDTOs(IEnumerable<ImageData> images)
        {
            List<ImageDataDTO> imageDTOs = new List<ImageDataDTO>();
            foreach (var image in images)
            {
                ImageDataDTO dto = new ImageDataDTO
                {
                    Id = image.Id,
                    ProductId = image.ProductId,
                    Name = image.Name,
                    Caption = image.Caption,
                    SequenceNumber = image.SequenceNumber,
                    ImageUrl = image.ImageUrl,
                    ThumbnailUrl = image.ThumbnailUrl
                };
                imageDTOs.Add(dto);
            }

            return imageDTOs;
        }

        private List<DocumentDataDTO> MapDocumentsToDTOs(IEnumerable<DocumentData> documents)
        {
            List<DocumentDataDTO> docDTOs = new List<DocumentDataDTO>();
            foreach (var doc in documents)
            {
                DocumentDataDTO dto = new DocumentDataDTO
                {
                    Id = doc.Id,
                    ProductId = doc.ProductId,
                    Name = doc.Name,
                    Title = doc.Title,
                    SequenceNumber = doc.SequenceNumber,
                    DocumentUrl = doc.DocumentUrl,
                };
                docDTOs.Add(dto);
            }
            return docDTOs;
        }

        public async Task<GetProductSummariesResult> GetAllProductSummariesAsync()
        {
            IEnumerable<Product> products = await _db.Products.Include(p => p.Images).Include(p => p.Documents).ToListAsync();
            List<ProductSummaryDTO> summaryDTOs = new List<ProductSummaryDTO>();
            foreach (var product in products)
            {
                ProductSummaryDTO dto = new ProductSummaryDTO
                {
                    Id = product.Id,
                    AggregateId = product.AggregateId,
                    Name = product.Name,
                    Category = product.Category,
                    Description = product.Description,
                    Price = product.Price,
                    Status = product.Status,
                    Version = product.Version,
                    DateCreated = product.DateCreated,
                    DateUpdated = product.DateUpdated,
                    ImageCount = product.Images is null ? 0 : product.Images.Count(),               
                    DocumentCount = product.Documents is null ? 0 : product.Documents.Count() 

                };
                summaryDTOs.Add(dto);
            }
            return new GetProductSummariesResult(true, summaryDTOs, null);

            //var result = await (
            //            from product in _db.Products
            //            join image in _db.ImageData on product.Id equals image.ProductId into images
            //            join document in _db.DocumentData on product.Id equals document.ProductId into documents
            //             select new  ProductSummaryDTO
            //                 {
            //                     Id = product.Id,
            //                     AggregateId = product.AggregateId,
            //                     Name = product.Name,
            //                     Category = product.Category,
            //                     Description = product.Description,
            //                     Price = product.Price,
            //                     Status = product.Status,
            //                     Version = product.Version,
            //                     DateCreated = product.DateCreated,
            //                     DateUpdated = product.DateUpdated,
            //                     ImageCount = images == null ? 0 : images.ToList().Count(), // 0,  // (from i in _db.ImageData where i.ProductId == product.Id select i).ToList().Count(),
            //                     DocumentCount = documents == null ? 0 : documents.ToList().Count() // 0,  // (from d in _db.DocumentData where d.ProductId == product.Id select d).ToList().Count()

            //                 }).ToListAsync();

            //return result;
        }

        public async Task<GetPagedAndFilteredProductSummariesResult> GetPagedAndFilteredProductSummariesAsync(string? filter, string? category, string? sortColumn, int pageNumber = 1, int pageSize = 10)
        {
            var query = _db.Products.Include(p => p.Images).Include(p => p.Documents).AsQueryable();

            if (!string.IsNullOrWhiteSpace(category)) query = query.Where(c => c.Category.ToLower() == category.ToLower());

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(p => p.Name.ToLower().Contains(filter.ToLower()) || p.Description.ToLower().Contains(filter.ToLower()));
            }

            switch (sortColumn?.ToLower())
            {
                case "id":
                    query = query.OrderBy(p => p.Id);
                    break;
                case "name":
                    query = query.OrderBy(p => p.Name);
                    break;
                case "price ascending":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "price descending":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                default:
                    query = query.OrderBy(p => p.Name);
                    break;
            }

            var products = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            List<ProductSummaryDTO> summaryDTOs = new List<ProductSummaryDTO>();
            foreach (var product in products)
            {
                ProductSummaryDTO dto = new ProductSummaryDTO
                {
                    Id = product.Id,
                    AggregateId = product.AggregateId,
                    Name = product.Name,
                    Category = product.Category,
                    Description = product.Description,
                    Price = product.Price,
                    Status = product.Status,
                    Version = product.Version,
                    DateCreated = product.DateCreated,
                    DateUpdated = product.DateUpdated,
                    ImageCount = product.Images is null ? 0 : product.Images.Count(),
                    DocumentCount = product.Documents is null ? 0 : product.Documents.Count()

                };
                summaryDTOs.Add(dto);
            }

            PaginationMetadata metadata = new PaginationMetadata(summaryDTOs.Count(), pageSize, pageNumber);
            return new GetPagedAndFilteredProductSummariesResult(true, summaryDTOs, metadata, null);
        }

        public async Task<GetProductSummaryByIdResult> GetProductSummaryByIdAsync(int id)
        {
            Product? product = await _db.Products.Include(p => p.Images).Include(p => p.Documents).FirstOrDefaultAsync(p => p.Id == id);
            if (product is null) return new GetProductSummaryByIdResult(false, null, $"A product with Id {id} was not found.");

            ProductSummaryDTO dto = new ProductSummaryDTO
            {
                Id = product.Id,
                AggregateId = product.AggregateId,
                Name = product.Name,
                Category = product.Category,
                Description = product.Description,
                Price = product.Price,
                Status = product.Status,
                Version = product.Version,
                DateCreated = product.DateCreated,
                DateUpdated = product.DateUpdated,
                ImageCount = product.Images is null ? 0 : product.Images.Count(),
                DocumentCount = product.Documents is null ? 0 : product.Documents.Count()

            };
            
            return new GetProductSummaryByIdResult(true, dto, null);
        }
    }
}
