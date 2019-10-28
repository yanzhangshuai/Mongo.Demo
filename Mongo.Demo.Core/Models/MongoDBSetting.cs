namespace Mongo.Demo.Core.Models
{
    public class MongoDBSetting
    {
        public string HostName { get; set; }

        public int Port { get; set; }

        public string DatabaseName { get; set; }

        public bool UseAuthentication { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}