using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Products.Write.API.Configuration;
using Products.Write.Application.Abstractions;
using Products.Write.Application.CQRS.DevTests;
using Products.Write.Domain.Aggregates;
using Products.Write.Domain.Enumerations;
using Products.Write.Application.CQRS.Commands;
using Products.Write.Application.CQRS.CommandResults;

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
    }
}
