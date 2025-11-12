namespace Carts.API.Abstractions
{
    public interface IMongoSettings
    {
        string? MongoLocalConnection { get; }
        string? Database { get; }
        string? ShoppingCartCollection { get; }
    }
}
