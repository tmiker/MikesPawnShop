using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Write.Application.Abstractions;
using Products.Write.Application.CQRS.CommandResults;
using Products.Write.Application.CQRS.Commands;

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
        public async Task<IActionResult> AddProduct([FromBody] AddProduct command, CancellationToken cancellationToken)
        {
            AddProductResult result = await _commandDispatcher.DispatchAsync<AddProduct, AddProductResult>(command, cancellationToken);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result.ErrorMessage);

        }
    }
}
