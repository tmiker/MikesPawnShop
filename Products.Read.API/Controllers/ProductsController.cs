using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Products.Read.API.Abstractions;
using Products.Read.API.Domain.Models;
using Products.Read.API.DTOs;
using Products.Read.API.Paging;
using Products.Read.API.QueryResponses;

namespace Products.Read.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductQueryService _productQueryService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductQueryService productQueryService, ILogger<ProductsController> logger)
        {
            _productQueryService = productQueryService;
            _logger = logger;
        }

        [HttpGet("productStream")]
        [AllowAnonymous]
        // [OutputCache(PolicyName = "NoCache")]
        public async IAsyncEnumerable<Product> StreamProducts()
        {
            await foreach (var product in _productQueryService.GetProductsAsAsyncEnumerable())
            {
                yield return product;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            GetProductsResult result = await _productQueryService.GetAllProductsAsync();
            if (result.IsSuccess) return Ok(result.Products);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("summaries")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductSummaryDTO>>> GetProductSummaries()
        {
            GetProductSummariesResult result = await _productQueryService.GetAllProductSummariesAsync();
            if (result.IsSuccess) return Ok(result.ProductSummaries);
            return BadRequest(result.ErrorMessage);
        }

        // ResponseCache Location:
        //   Any - both the client and server will be able to cache the response, which is equivalent to the public directive of the cache-control header
        //   Client - changes the cache-control header value to private which means that only the client can cache the response
        //   None - sets both the cache-control and pragma header to no-cache, which means the client cannot use a cached response without revalidating with the server 

        [HttpGet("paged")]
        [AllowAnonymous]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        // [OutputCache(PolicyName = "SixtySecondsCache")]
        public async Task<ActionResult<PagedProductsDTO>> GetPagedAndFilteredProducts(string? filter, string? category, string? sortColumn, int pageNumber = 1, int pageSize = 10)
        {
            GetPagedAndFilteredProductsResult result = await _productQueryService.GetPagedAndFilteredProductsAsync(filter, category, sortColumn, pageNumber, pageSize);
            if (result.IsSuccess) return Ok(new PagedProductsDTO { Products = result.Products, PagingData = result.PaginationMetadata, FetchTime = DateTime.Now });
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("paged/summaries")]
        [AllowAnonymous]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        // [OutputCache(PolicyName = "SixtySecondsCache")]
        public async Task<ActionResult<PagedProductSummariesDTO>> GetPagedAndFilteredProductSummaries(string? filter, string? category, string? sortColumn, int pageNumber = 1, int pageSize = 10)
        {
            GetPagedAndFilteredProductSummariesResult result = await _productQueryService.GetPagedAndFilteredProductSummariesAsync(filter, category, sortColumn, pageNumber, pageSize);
            if (result.IsSuccess) return Ok(new PagedProductSummariesDTO { ProductSummaries = result.ProductSummaries, PagingData = result.PaginationMetadata, FetchTime = DateTime.Now });
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new string[] { "id" })]
        // [OutputCache(PolicyName = "SixtySecondsCache")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            GetProductByIdResult result = await _productQueryService.GetProductByIdAsync(id);
            if (result.IsSuccess)
            {
                if (result.Product is not null) result.Product.FetchTime = DateTime.Now;
                return Ok(result.Product);
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("summary/{id}")]
        [AllowAnonymous]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new string[] { "id" })]
        // [OutputCache(PolicyName = "SixtySecondsCache")]
        public async Task<ActionResult<ProductSummaryDTO>> GetProductSummaryById(int id)
        {
            GetProductSummaryByIdResult result = await _productQueryService.GetProductSummaryByIdAsync(id);
            if (result.IsSuccess)
            {
                if (result.ProductSummary is not null) result.ProductSummary.FetchTime = DateTime.Now;
                return Ok(result.ProductSummary);
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
