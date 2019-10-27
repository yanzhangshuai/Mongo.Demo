namespace Mongo.Demo.Core.Repository
{
    public interface IEntity<T>
    {
         T Id { get; set; }
    }

    public interface IEntity : IEntity<int>
    {
        
    }
}