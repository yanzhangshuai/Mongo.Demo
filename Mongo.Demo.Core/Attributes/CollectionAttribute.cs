using System;

namespace Mongo.Demo.Core.Attributes
{
    /// <inheritdoc />
    /// <summary>Specifies the database table that a class is mapped to.</summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionAttribute : Attribute
    {
        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Mongo.Demo.Core.CollectionAttribute" />
        ///     class using the specified name of the collection.
        /// </summary>
        /// <param name="name">The name of the table the class is mapped to.</param>
        public CollectionAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(Name));
            Name = name;
        }

        /// <summary>Gets the name of the table the class is mapped to.</summary>
        /// <returns>The name of the table the class is mapped to.</returns>
        public string Name { get; }
    }
}