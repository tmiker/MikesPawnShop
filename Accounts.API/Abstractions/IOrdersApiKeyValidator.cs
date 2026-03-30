namespace Accounts.API.Abstractions
{
    public interface IOrdersApiKeyValidator
    {
        bool ApiKeyIsValid(string apiKeyFromHeader);
    }
}
