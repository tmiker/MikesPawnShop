using Carts.API.Abstractions;

namespace Carts.API.Infrastructure.Mongo
{
    public class MongoSettings : IMongoSettings
    {
        public string? MongoLocalConnection { get; set; }
        public string? Database { get; set; }
        public string? ShoppingCartCollection { get; set; }
    }
}

