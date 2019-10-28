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
        private readonly MongoDBSetting _setting;

        public DefaultMongoDatabaseProvider(
            [NotNull] MongoDBSetting setting
        )
        {
            _setting = setting ?? throw new ArgumentNullException(nameof(_setting));
            _client = new Lazy<IMongoClient>(() =>
            {
                var clientSetting = new MongoClientSettings()
                {
                    Server = new MongoServerAddress(setting.HostName, setting.Port),
                    ClusterConfigurator = cb =>
                    {
                        //      监听执行语句
                        cb.Subscribe<CommandStartedEvent>(hand =>
                        {
                            Console.WriteLine($"命令名称:{hand.CommandName};命令内容{hand.Command}");
                        });
                    }
                };
                if (setting.UseAuthentication)
                {
                    var credential = MongoCredential.CreateCredential(setting.DatabaseName,
                        setting.UserName, setting.Password);
                    clientSetting.Credential = credential;
                }

                return new MongoClient(clientSetting);
            });
        }

        public IMongoDatabase Database => _client.Value.GetDatabase(_setting.DatabaseName);
    }
}