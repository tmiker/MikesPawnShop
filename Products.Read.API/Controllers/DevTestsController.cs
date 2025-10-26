using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Products.Read.API.Configuration;

namespace Products.Read.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevTestsController : ControllerBase
    {
        private readonly IOptions<CloudAMQPSettings> _cloudAmqpSettings;
        private readonly ILogger<DevTestsController> _logger;

        public DevTestsController(IOptions<CloudAMQPSettings> cloudAmqpSettings, ILogger<DevTestsController> logger)
        {
            _cloudAmqpSettings = cloudAmqpSettings;
            _logger = logger;
        }

        [HttpGet("getCloudAmqpSettingsTestingDummyValue")]
        public IActionResult GetCloudAmqpTestingDummyValue(CancellationToken cancellationToken)
        {
            string? value = _cloudAmqpSettings.Value.TestingDummyValue;
            if (!string.IsNullOrWhiteSpace(value)) return Ok(value);
            return BadRequest("Unable to find the CloudAMQPSettings TestingDummyValue.");
        }
    }
}
