using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mongo.Demo.log;

namespace Mongo.Demo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new HostBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddEnvironmentVariables();
                    config.AddJsonFile("appsettings.json", true, false);
                })
                .ConfigureLogging((context, builder) =>
                {
                    builder.AddLog4Net(true);
                    builder.SetMinimumLevel(LogLevel.Debug);
                })
                .ConfigureServices((context, service) =>
                {
                    DiBuilder.Build(service, context.Configuration);
                    service.AddHostedService<MongoHostedService>();
                }).RunConsoleAsync();
        }
    }
}