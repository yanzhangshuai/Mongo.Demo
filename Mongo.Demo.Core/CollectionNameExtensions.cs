using System;
using System.Linq;

namespace Mongo.Demo.Core.Repository
{
    public static class CollectionNameExtensions
    {
        public static string GetCollectionName(this Type type)
        {
            if (type.GetCustomAttributes(typeof(CollectionAttribute), true).FirstOrDefault() is CollectionAttribute attribute)
            {
                return attribute.Name;
            }

            return type.Name;
        }
    }
}