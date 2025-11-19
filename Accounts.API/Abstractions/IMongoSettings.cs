namespace Accounts.API.Abstractions
{
    public interface IMongoSettings
    {
        string? MongoLocalConnection { get; }
        string? Database { get; }
        string? AccountCollection { get; }
    }
}
