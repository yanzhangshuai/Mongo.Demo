using MongoDB.Driver;

namespace Mongo.Demo.Core.provider
{
    public interface IMongoDatabaseProvider
    {
        IMongoDatabase Database { get; }
    }
}