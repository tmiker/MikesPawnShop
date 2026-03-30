using Accounts.API.Abstractions;
using Accounts.API.Utility;

namespace Accounts.API.Filters
{
    public class OrdersApiKeyValidator : IOrdersApiKeyValidator
    {
        private readonly IConfiguration _config;
        private readonly ILogger<OrdersApiKeyValidator> _logger;

        public OrdersApiKeyValidator(IConfiguration config, ILogger<OrdersApiKeyValidator> logger)
        {
            _config = config;
            _logger = logger;
        }

        public bool ApiKeyIsValid(string apiKeyFromHeader)
        {
            if (string.IsNullOrWhiteSpace(apiKeyFromHeader))
            {
                _logger.LogError("InternalAccountsServices ExtOrdersApiKeyValidator API-Key Validation Failure: Value from header is null or whitespace.");
                return false;
            }

            string? apiKey = _config.GetValue<string>(StaticData.OrdersToAccountsApiKeyName);
            if (apiKey is null || apiKey != apiKeyFromHeader)
            {
                _logger.LogError($"InternalAccountsServices ExtOrdersApiKeyValidator API-Key Validation Failure: Value from header does not match Value from secrets.");
                return false;
            }

            _logger.LogInformation($"InternalAccountsServices ExtOrdersApiKeyValidator API-Key Validation Success: Value from header matches Value from secrets.");
            return true;
        }

    }
}
