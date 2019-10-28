using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Mongo.Demo
{
    public class MongoHostedService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private ILogger<MongoHostedService> _logger;

        public MongoHostedService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await new ValueTask();
            var scope = _serviceScopeFactory.CreateScope();
            var startup = scope.ServiceProvider.GetRequiredService<Startup>();
            _logger = scope.ServiceProvider.GetService<ILogger<MongoHostedService>>();
            //    调用startup执行项目代码
            startup.Start(scope);
            Console.WriteLine("started.........");
            _logger.LogInformation("started........");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("begin to stop");
            _logger.LogInformation("begin to stop ");
            Console.WriteLine("stopped.....");
            _logger.LogInformation("stopped....");
            await new ValueTask();
        }
    }
}