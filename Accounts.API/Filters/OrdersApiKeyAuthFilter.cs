using Accounts.API.Abstractions;
using Accounts.API.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Accounts.API.Filters
{
    public class OrdersApiKeyAuthFilter : IAuthorizationFilter
    {
        private readonly IOrdersApiKeyValidator _apiKeyValidator;

        public OrdersApiKeyAuthFilter(IOrdersApiKeyValidator apiKeyValidator)
        {
            _apiKeyValidator = apiKeyValidator;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string apiKeyFromHeader = context.HttpContext.Request.Headers[StaticData.OrdersToAccountsApiKeyHeaderName].ToString();

            if (string.IsNullOrWhiteSpace(apiKeyFromHeader))
            {
                context.Result = new BadRequestResult();
                return;
            }

            if (!_apiKeyValidator.ApiKeyIsValid(apiKeyFromHeader))
            {
                context.Result = new UnauthorizedResult();
            }
        }

    }
}
