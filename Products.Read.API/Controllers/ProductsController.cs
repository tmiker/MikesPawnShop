using Microsoft.AspNetCore.Mvc;
using Products.Read.API.Abstractions;
using Products.Read.API.Domain.Models;
using Products.Read.API.DTOs;
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
        public async IAsyncEnumerable<Product> StreamProducts()
        {
            await foreach (var product in _productQueryService.GetProductsAsAsyncEnumerable())
            {
                yield return product;
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            GetProductsResult result = await _productQueryService.GetAllProductsAsync();
            if (result.IsSuccess) return Ok(result.Products);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("summaries")]
        public async Task<ActionResult<IEnumerable<ProductSummaryDTO>>> GetProductSummaries()
        {
            GetProductSummariesResult result = await _productQueryService.GetAllProductSummariesAsync();
            if (result.IsSuccess) return Ok(result.ProductSummaries);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetPagedAndFilteredProducts(string? filter, string? category, string? sortColumn, int pageNumber = 1, int pageSize = 10)
        {
            GetPagedAndFilteredProductsResult result = await _productQueryService.GetPagedAndFilteredProductsAsync(filter, category, sortColumn, pageNumber, pageSize);
            if (result.IsSuccess) return Ok(result.Products);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("paged/summaries")]
        public async Task<ActionResult<IEnumerable<ProductSummaryDTO>>> GetPagedAndFilteredProductSummaries(string? filter, string? category, string? sortColumn, int pageNumber = 1, int pageSize = 10)
        {
            GetPagedAndFilteredProductSummariesResult result = await _productQueryService.GetPagedAndFilteredProductSummariesAsync(filter, category, sortColumn, pageNumber, pageSize);
            if (result.IsSuccess) return Ok(result.ProductSummaries);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            GetProductByIdResult result = await _productQueryService.GetProductByIdAsync(id);
            if (result.IsSuccess) return Ok(result.Product);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("summary/{id}")]
        public async Task<ActionResult<ProductSummaryDTO>> GetProductSummaryById(int id)
        {
            GetProductSummaryByIdResult result = await _productQueryService.GetProductSummaryByIdAsync(id);
            if (result.IsSuccess) return Ok(result.ProductSummary);
            return BadRequest(result.ErrorMessage);
        }
    }
}
