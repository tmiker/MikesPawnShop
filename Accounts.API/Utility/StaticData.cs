namespace Accounts.API.Utility
{
    public class StaticData
    {
        // *** EXTERNAL ORDERS API-Key Authentication *** //

        /// INCOMING (to this api - i.e. out from orders.api)
        public const string OrdersToAccountsApiKeyHeaderName = "X-OrdersToAccounts-API-Key";
        public const string OrdersToAccountsApiKeyName = "OrdersToAccountsApiKey";


        // app secrets:  OrdersToAccountsApiKey
        //      from InternalAccountsServices.Api directory:  dotnet user-secrets set "ExtOrdersToIntAccountsApiKey" "---value---"  1

    }
}
