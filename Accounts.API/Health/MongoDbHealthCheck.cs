using Accounts.API.Abstractions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Accounts.API.Health
{
    public class MongoDbHealthCheck : IHealthCheck
    {
        private readonly IMongoDatabase _database;

        public MongoDbHealthCheck(IMongoSettings mongoSettings)
        {
            var client = new MongoClient(mongoSettings.MongoLocalConnection);
            _database = client.GetDatabase(mongoSettings.Database);
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                await _database.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
                return HealthCheckResult.Healthy("MongoDB is healthy");
            }
            catch
            {
                return HealthCheckResult.Unhealthy("MongoDB is unreachable");
            }
        }
    }
}
