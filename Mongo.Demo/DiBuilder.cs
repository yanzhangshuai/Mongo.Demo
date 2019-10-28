using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Demo.Core;
using Mongo.Demo.User;

namespace Mongo.Demo
{
    public static class DiBuilder
    {
        public static void Build(IServiceCollection services, IConfiguration configuration)
        {
            var con = configuration.GetSection("MongoDBConnection");
            services.AddMongo(option =>
            {
                option.HostName = con["HostName"];
                option.Port = int.Parse(con["Port"]);
                option.UseAuthentication = false;
                option.DatabaseName = con["DatabaseName"];
            });
            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<Startup>();
        }

//        public static void A()
//        {
//                typeof(ITransientDependency).get
//        }
    }
}