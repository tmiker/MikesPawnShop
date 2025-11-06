using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Products.Write.Application.Abstractions;
using Products.Write.Application.Configuration;
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
        private readonly IOptions<AzureSettings> _azureSettings;
        private readonly ILogger<ProductsManagementController> _logger;

        public ProductsManagementController(ICommandDispatcher commandDispatcher, IOptions<AzureSettings> azureSettings, ILogger<ProductsManagementController> logger)
        {
            _commandDispatcher = commandDispatcher;
            _azureSettings = azureSettings;
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

        [HttpPost("image")]
        public async Task<IActionResult> AddImage([FromForm] AddImageDTO addImageDTO, CancellationToken cancellationToken)
        {
            var correlationId = HttpContext.Request.Headers["X-Correlation-ID"];
            AddImage command = new AddImage(addImageDTO, correlationId);
            AddImageResult result = await _commandDispatcher.DispatchAsync<AddImage, AddImageResult>(command, cancellationToken);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("document")]
        public async Task<IActionResult> AddDocument([FromForm] AddDocumentDTO addDocumentDTO, CancellationToken cancellationToken)
        {
            if (addDocumentDTO.DocumentBlob is null) Console.WriteLine($"The Document Blob is null.");
            var correlationId = HttpContext.Request.Headers["X-Correlation-ID"];
            AddDocument command = new AddDocument(addDocumentDTO, correlationId);
            AddDocumentResult result = await _commandDispatcher.DispatchAsync<AddDocument, AddDocumentResult>(command, cancellationToken);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusDTO updateStatusDTO, CancellationToken cancellationToken)
        {
            var correlationId = HttpContext.Request.Headers["X-Correlation-ID"];
            UpdateStatus command = new UpdateStatus(updateStatusDTO, correlationId);
            UpdateStatusResult result = await _commandDispatcher.DispatchAsync<UpdateStatus, UpdateStatusResult>(command, cancellationToken);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("image")]
        public async Task<IActionResult> DeleteImage(DeleteImageDTO deleteImageDTO, CancellationToken cancellationToken)
        {
            var correlationId = HttpContext.Request.Headers["X-Correlation-ID"];

            DeleteImage command = new DeleteImage(deleteImageDTO, correlationId);
            DeleteImageResult result = await _commandDispatcher.DispatchAsync<DeleteImage, DeleteImageResult>(command, cancellationToken);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result.ErrorMessage);
        }
        [HttpDelete("document")]
        public async Task<IActionResult> DeleteDocument(DeleteDocumentDTO deleteDocumentDTO, CancellationToken cancellationToken)
        {
            var correlationId = HttpContext.Request.Headers["X-Correlation-ID"];
            DeleteDocument command = new DeleteDocument(deleteDocumentDTO, correlationId);
            DeleteDocumentResult result = await _commandDispatcher.DispatchAsync<DeleteDocument, DeleteDocumentResult>(command, cancellationToken);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result.ErrorMessage);
        }
    }
}
