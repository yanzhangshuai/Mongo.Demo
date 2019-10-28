using System;
using Mongo.Demo.Core;
using Mongo.Demo.Core.Attributes;

namespace Mongo.Demo.User
{
    [Collection("Users")]
    public class User : Entity<int>
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public DateTime BornDateTime { get; set; }
    }
}