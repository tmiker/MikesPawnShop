using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Products.Write.API.Configuration;
using Products.Write.Application.Abstractions;
using Products.Write.Application.CQRS.DevTests;
using Products.Write.Application.CQRS.QueryResults;
using Products.Write.Application.DTOs;
using Products.Write.Domain.Snapshots;

namespace Products.Write.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevTestsController : ControllerBase
    {
        private readonly IDevQueryService _devQueryService;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IOptions<CloudAMQPSettings> _cloudAmqpSettings;
        private readonly ILogger<DevTestsController> _logger;

        public DevTestsController(IDevQueryService devQueryService , ICommandDispatcher commandDispatcher, IOptions<CloudAMQPSettings> cloudAmqpSettings, ILogger<DevTestsController> logger)
        {
            _devQueryService = devQueryService;
            _commandDispatcher = commandDispatcher;
            _cloudAmqpSettings = cloudAmqpSettings;
            _logger = logger;
        }

        // Query service propagated endpoints

        [HttpGet("productSnapshots")]
        public async Task<ActionResult<PagedProductSnapshotResult>> GetProductSnapshots(
            string? aggregateId,
            int minVersion = 0,
            int maxVersion = Int32.MaxValue,
            int pageNumber = 1,
            int pageSize = 10)
        {
            Guid? guid = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(aggregateId)) guid = Guid.Parse(aggregateId);
            var result = await _devQueryService.GetProductSnapshotsAsync(guid, minVersion, maxVersion, pageNumber, pageSize);
            if (result.IsSuccess) return Ok(new PagedProductSnapshotResult() { ProductSnapshots = result.ProductSnapshots, PagingData = result.PagingData });
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("productSnapshot/{aggregateId}")]
        public async Task<ActionResult<ProductSnapshot>> GetProductSnapshot(
            string? aggregateId,
            int minVersion = 0,
            int maxVersion = Int32.MaxValue)
        {
            Guid guid = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(aggregateId)) guid = Guid.Parse(aggregateId);
            var result = await _devQueryService.GetProductSnapshotByIdAsync(guid, minVersion, maxVersion);
            if (result.IsSuccess) return Ok(result.ProductSnapshot);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("eventRecords")]
        public async Task<ActionResult<PagedEventRecordResult>>  GetEventRecords(
            string? aggregateId,
            string? correlationId = null,
            int minVersion = 0,
            int maxVersion = Int32.MaxValue,
            int pageNumber = 1,
            int pageSize = 10)
        {
            Guid? guid = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(aggregateId)) guid = Guid.Parse(aggregateId);
            var result = await _devQueryService.GetEventRecordsAsync(guid, correlationId, minVersion, maxVersion, pageNumber, pageSize);
            if (result.IsSuccess)
            {
                _logger.LogInformation("The Dev Tests Query Service returned {count} EventRecords.", result.EventRecords?.Count());
                return Ok(new PagedEventRecordResult() { EventRecords = result.EventRecords, PagingData = result.PagingData });
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("outboxRecords")]
        public async Task<ActionResult<PagedOutboxRecordResult>> GetOutboxRecords(
            string? aggregateId,
            string? correlationId = null,
            int minVersion = 0,
            int maxVersion = Int32.MaxValue,
            int pageNumber = 1,
            int pageSize = 10)
        {
            Guid? guid = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(aggregateId)) guid = Guid.Parse(aggregateId);
            var result = await _devQueryService.GetOutboxRecordsAsync(guid, correlationId, minVersion, maxVersion, pageNumber, pageSize);
            if (result.IsSuccess) return Ok(new PagedOutboxRecordResult() { OutboxRecords = result.OutboxRecords, PagingData = result.PagingData });
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("snapshotRecords")]
        public async Task<ActionResult<PagedSnapshotRecordResult>> GetSnapshotRecords(
            string? aggregateId,
            string? correlationId = null,
            int minVersion = 0,
            int maxVersion = Int32.MaxValue,
            int pageNumber = 1,
            int pageSize = 10)
        {
            Guid? guid = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(aggregateId)) guid = Guid.Parse(aggregateId);
            var result = await _devQueryService.GetSnapshotRecordsAsync(guid, correlationId, minVersion, maxVersion, pageNumber, pageSize);
            if (result.IsSuccess) return Ok(new PagedSnapshotRecordResult() { SnapshotRecords = result.SnapshotRecords, PagingData = result.PagingData });
            return BadRequest(result.ErrorMessage);
        }

        // Command propagated endpoints

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

        [HttpPost("purgeData")]
        public async Task<IActionResult> PurgeData([FromBody] PurgeDataDTO purgeDataDTO, CancellationToken cancellationToken)
        {
            var correlationId = HttpContext.Request.Headers["X-Correlation-ID"];
            PurgeData command = new PurgeData(purgeDataDTO.PinNumber, correlationId);
            PurgeDataResult result = await _commandDispatcher.DispatchAsync<PurgeData, PurgeDataResult>(command, cancellationToken);
            if (result.IsSuccess) return Ok();
            return BadRequest(result.ErrorMessage);
        }
    }
}
