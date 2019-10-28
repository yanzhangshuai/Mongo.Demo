using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Demo.User;

namespace Mongo.Demo
{
    public class Startup
    {
        public void Start(IServiceScope serviceScope)
        {
            var manager = serviceScope.ServiceProvider.GetService<IUserManager>();
            manager.Inert().GetAwaiter().GetResult();
            Console.WriteLine(manager.GetUsers().Count());
            Console.WriteLine(manager.GetUsers2().GetAwaiter().GetResult().Count);
            Console.WriteLine(manager.GetCount());
            ;
        }
    }
}