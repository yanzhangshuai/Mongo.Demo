using System;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Demo.Core.Models;
using Mongo.Demo.Core.provider;
using Mongo.Demo.Core.Repository;

namespace Mongo.Demo.Core
{
    public static class ServiceCollectionExtension
    {
        public static void AddMongo(this IServiceCollection service, Action<MongoDBSetting> options)
        {
            service.AddTransient(typeof(IMongoRepository<,>), typeof(MongoRepositoryBase<,>));
            var setting = new MongoDBSetting();
            options(setting);
            service.AddSingleton<IMongoDatabaseProvider>(new DefaultMongoDatabaseProvider(setting));
        }
    }
}