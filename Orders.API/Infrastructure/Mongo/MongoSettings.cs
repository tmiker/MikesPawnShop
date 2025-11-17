using Orders.API.Abstractions;

namespace Orders.API.Infrastructure.Mongo
{
    public class MongoSettings : IMongoSettings
    {
        public string? MongoLocalConnection { get; set; }
        public string? Database { get; set; }
        public string? OrderCollection { get; set; }
    }
}
