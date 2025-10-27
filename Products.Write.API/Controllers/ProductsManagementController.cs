using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Write.Application.Abstractions;
using Products.Write.Application.CQRS.CommandResults;
using Products.Write.Application.CQRS.Commands;
using Products.Write.Application.DTOs;

namespace Products.Write.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsManagementController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ILogger<ProductsManagementController> _logger;

        public ProductsManagementController(ICommandDispatcher commandDispatcher, ILogger<ProductsManagementController> logger)
        {
            _commandDispatcher = commandDispatcher;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] AddProductDTO addProductDTO, CancellationToken cancellationToken)
        {
            // Note passing Correlation ID from the request headers to the command as Microsoft recommends
            // caution using IHttpContextAccessor to get http context if want to pull header in handlers
            // (https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.ihttpcontextaccessor?view=aspnetcore-9.0).
            var correlationId = HttpContext.Request.Headers["X-Correlation-ID"];
            AddProduct command = new AddProduct(addProductDTO, correlationId);
            // AddProductResult result = await _commandManagementService.ExecuteCommandAsync<AddProduct, AddProductResult>(command, cancellationToken);
            AddProductResult result = await _commandDispatcher.DispatchAsync<AddProduct, AddProductResult>(command, cancellationToken);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result.ErrorMessage);
        }
    }
}
