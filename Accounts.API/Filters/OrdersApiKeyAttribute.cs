using Microsoft.AspNetCore.Mvc;

namespace Accounts.API.Filters
{
    public class OrdersApiKeyAttribute : ServiceFilterAttribute	        // use attribute [OrdersApiKey] in controller
    {
        public OrdersApiKeyAttribute() : base(typeof(OrdersApiKeyAuthFilter))
        {
        }
    }

}
