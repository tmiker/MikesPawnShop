namespace Orders.API.Abstractions
{
    public interface IMongoSettings
    {
        string? MongoLocalConnection { get; }
        string? Database { get; }
        string? OrderCollection { get; }
    }
}
