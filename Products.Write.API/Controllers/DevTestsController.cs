using Microsoft.AspNetCore.Mvc;
using Products.Write.Application.Abstractions;
using Products.Write.Application.CQRS.DevTests;

namespace Products.Write.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevTestsController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ILogger<DevTestsController> _logger;

        public DevTestsController(ICommandDispatcher commandDispatcher, ILogger<DevTestsController> logger)
        {
            _commandDispatcher = commandDispatcher;
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
    }
}
