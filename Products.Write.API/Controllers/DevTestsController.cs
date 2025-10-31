using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Products.Write.API.Configuration;
using Products.Write.Application.Abstractions;
using Products.Write.Application.CQRS.CommandResults;
using Products.Write.Application.CQRS.Commands;
using Products.Write.Application.CQRS.DevTests;
using Products.Write.Application.DTOs;
using Products.Write.Domain.Aggregates;
using Products.Write.Domain.Enumerations;

namespace Products.Write.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevTestsController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IOptions<CloudAMQPSettings> _cloudAmqpSettings;
        private readonly ILogger<DevTestsController> _logger;

        public DevTestsController(ICommandDispatcher commandDispatcher, IOptions<CloudAMQPSettings> cloudAmqpSettings, ILogger<DevTestsController> logger)
        {
            _commandDispatcher = commandDispatcher;
            _cloudAmqpSettings = cloudAmqpSettings;
            _logger = logger;
        }

        [HttpPost("throwExceptionForTesting")]
        public async Task<IActionResult> ThrowExceptionForTesting([FromBody] ThrowExceptionDTO throwExceptionDTO, CancellationToken cancellationToken)
        {
            // Note passing Correlation ID from the request headers to the command as Microsoft recommends
            // caution using IHttpContextAccessor to get http context if want to pull header in handlers
            // (https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.ihttpcontextaccessor?view=aspnetcore-9.0).

            var correlationId = HttpContext.Request.Headers["X-Correlation-ID"];
            ThrowException command = new ThrowException(throwExceptionDTO, correlationId);
            ThrowExceptionResult result = await _commandDispatcher.DispatchAsync<ThrowException, ThrowExceptionResult>(command, cancellationToken);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result.ErrorMessage);
        }


        [HttpGet("getCloudAmqpSettingsTestingDummyValue")]
        public IActionResult GetCloudAmqpTestingDummyValue(CancellationToken cancellationToken)
        {
            string? value = _cloudAmqpSettings.Value.TestingDummyValue;
            if (!string.IsNullOrWhiteSpace(value)) return Ok(value);
            return BadRequest("Unable to find the CloudAMQPSettings TestingDummyValue.");
        }

        [HttpPost("processMultipleDomainEvents")]
        public async Task<IActionResult> ProcessMulitpleEvents(CancellationToken cancellationToken)
        {
            string correlationId = Guid.NewGuid().ToString();
            ProcessMultipleEvents command = new ProcessMultipleEvents();

            var result = await _commandDispatcher.DispatchAsync<ProcessMultipleEvents, ProcessMultipleEventsResult>(command, cancellationToken);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("testAddProduct1")]
        public async Task<IActionResult> TestAddProduct1(CancellationToken cancellationToken)
        {
            var correlationId = HttpContext.Request.Headers["X-Correlation-ID"];
            AddProductDTO addProductDTO = new AddProductDTO("Test Product", "Astronomy", "A test product", 19.99m, "USD", "Active");
            AddProduct command = new AddProduct(addProductDTO, correlationId);
            AddProductResult result = await _commandDispatcher.DispatchAsync<AddProduct, AddProductResult>(command, cancellationToken);
            Console.WriteLine($"************** TEST ADD PRODUCT AGGREGATE ID: {result.ProductId} ****************");
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("testUpdateStatus/{aggregateId}")]
        public async Task<IActionResult> TestUpdateStatusProduct1(Guid aggregateId, CancellationToken cancellationToken)
        {
            var correlationId = HttpContext.Request.Headers["X-Correlation-ID"];
            UpdateStatus command = new UpdateStatus() { Status = "InActive", ProductId = aggregateId, CorrelationId = correlationId };
            UpdateStatusResult result = await _commandDispatcher.DispatchAsync<UpdateStatus, UpdateStatusResult>(command, cancellationToken);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("testAddImage/{aggregateId}")]
        public async Task<IActionResult> TestAddImage(Guid aggregateId, CancellationToken cancellationToken)
        {
            var correlationId = HttpContext.Request.Headers["X-Correlation-ID"];
            AddImage command = new AddImage() { Name = "Image Name", Caption = "Image Caption", SequenceNumber = 99, ProductId = aggregateId, ImageUrl = "I-URL", ThumbnailUrl = "T-URL", CorrelationId = correlationId };
            AddImageResult result = await _commandDispatcher.DispatchAsync<AddImage, AddImageResult>(command, cancellationToken);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("testAddDocument/{aggregateId}")]
        public async Task<IActionResult> TestAddDocument(Guid aggregateId, CancellationToken cancellationToken)
        {
            var correlationId = HttpContext.Request.Headers["X-Correlation-ID"];
            AddDocument command = new AddDocument() { Name = "Document Name", Title = "Docuument Title", SequenceNumber = 7, ProductId = aggregateId, DocumentUrl = "D-URL", CorrelationId = correlationId };
            AddDocumentResult result = await _commandDispatcher.DispatchAsync<AddDocument, AddDocumentResult>(command, cancellationToken);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result.ErrorMessage);
        }
    }
}
