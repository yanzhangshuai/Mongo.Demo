using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using Mongo.Demo.Core.Models;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace Mongo.Demo.Core.provider
{
    public class DefaultMongoDatabaseProvider : IMongoDatabaseProvider
    {
        private readonly Lazy<IMongoClient> _client;
        private readonly IOptions<MongoDbSetting> _mongoSetting;

        public DefaultMongoDatabaseProvider(
            [NotNull] IOptions<MongoDbSetting> mongoSetting
        )
        {
            _mongoSetting = mongoSetting ?? throw new ArgumentNullException(nameof(_mongoSetting));
            _client = new Lazy<IMongoClient>(() =>
            {
                return new MongoClient(new MongoClientSettings()
                {
                    Server = new MongoServerAddress(mongoSetting.Value.Address, mongoSetting.Value.Port),
                    ClusterConfigurator = cb =>
                    {
                        //      监听执行语句
                        cb.Subscribe<CommandStartedEvent>(hand =>
                        {
                            Console.WriteLine($"命令名称:{hand.CommandName};命令内容{hand.Command}");
                        });
                    }
                });
            });
        }

        public IMongoDatabase Database => _client.Value.GetDatabase(_mongoSetting.Value.DataBase);
    }
}