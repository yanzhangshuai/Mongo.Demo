using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mongo.Demo.Core.provider;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongo.Demo.Core.Repository
{
    /// <summary>
    ///     Implements IRepository for MongoDB.
    /// </summary>
    /// <typeparam name="TEntity">Type of the Entity for this repository</typeparam>
    public class MongoRepositoryBase<TEntity> : MongoRepositoryBase<TEntity, ObjectId>, IMongoRepository<TEntity> where TEntity : class, IEntity<ObjectId>
    {
        public MongoRepositoryBase(IMongoDatabaseProvider databaseProvider) : base(databaseProvider)
        {
            
        }
    }

    public class MongoRepositoryBase<TEntity, TPrimaryKey> : IMongoRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        private readonly IMongoDatabaseProvider _databaseProvider;
        public MongoRepositoryBase(IMongoDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }
        
        private IMongoDatabase Database => _databaseProvider.Database;

        public IMongoCollection<TEntity> Collection => Database.GetCollection<TEntity>(typeof(TEntity).GetCollectionName());

        public IQueryable<TEntity> GetAll()
        {
            //    此写法暂时没有异步
            return Collection.AsQueryable();
        }

        public virtual IList<TEntity> GetAllList()
        {
            return GetAll().ToList();
        }

        public virtual IList<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return Collection.Find(predicate).ToList();
        }

        public virtual async Task<IList<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.Find(predicate).ToListAsync();
        }

        public virtual T Query<T>(Func<IQueryable<TEntity>, T> queryMethod)
        {
            return queryMethod(GetAll());
        }

        public virtual TEntity Get(TPrimaryKey id)
        {
            var entity = FirstOrDefault(id);
            if (entity == null)throw new EntityNotFoundException("There is no such an entity with given primary key. Entity type: " + typeof(TEntity).FullName + ", primary key: " + id);
            return entity;
        }

        public virtual async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            var entity = await FirstOrDefaultAsync(id);
            if (entity == null) throw new EntityNotFoundException(typeof(TEntity), id);
            return entity;
        }

        public virtual TEntity FirstOrDefault(TPrimaryKey id)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
            return Collection.Find(filter).FirstOrDefault();
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
            return Collection.Find(filter).FirstOrDefaultAsync();
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Collection.Find(predicate).FirstOrDefaultAsync();
        }

        public virtual IList<TOutEntity> Aggregate<TOutEntity>(PipelineDefinition<TEntity, TOutEntity> pipelineDefinition, AggregateOptions options = null)
        {
            return Collection.Aggregate(pipelineDefinition,options).ToList();
        }

        public virtual TEntity Insert(TEntity entity)
        {
            Collection.InsertOne(entity, new InsertOneOptions());
            return entity;
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            await Collection.InsertOneAsync(entity);
            return entity;
        }

        public virtual void InsertMany(IEnumerable<TEntity> entities)
        {
             Collection.InsertMany(entities);
        }

        public virtual async Task InsertManyAsync(IEnumerable<TEntity> entities)
        {
            await Collection.InsertManyAsync(entities);
        }

        public virtual void ReplaceOne(FilterDefinition<TEntity> filter, TEntity entity)
        {
            Collection.ReplaceOne(filter, entity);
        }

        public virtual async Task ReplaceOneAsync(FilterDefinition<TEntity> filter, TEntity entity)
        {
            await Collection.ReplaceOneAsync(filter, entity);
        }

        public virtual TPrimaryKey InsertAndGetId(TEntity entity)
        {
            return Insert(entity).Id;
        }

        public virtual async  Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            return (await InsertAsync(entity)).Id;
        }

        public virtual TEntity Update(TEntity entity,UpdateOptions options = null)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            Collection.ReplaceOne(filter, entity,options);
            return entity;
        }

        public virtual async  Task<TEntity> UpdateAsync(TEntity entity,UpdateOptions options = null)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            await Collection.ReplaceOneAsync(filter, entity,options);
            return entity;
        }

        public virtual TEntity Update(TPrimaryKey id, Action<TEntity> updateAction)
        {
            var entity = Get(id);
            updateAction(entity);
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction,UpdateOptions options = null)
        {
            var entity = await GetAsync(id);
            await updateAction(entity);
            return entity;
        }

        public virtual void Update(Expression<Func<TEntity, bool>> filter, UpdateDefinition<TEntity> update,UpdateOptions options = null)
        {
            Collection.UpdateMany(filter, update,options);
        }

        public virtual async Task UpdateAsync(Expression<Func<TEntity, bool>> filter, UpdateDefinition<TEntity> update,UpdateOptions options = null)
        {
            await Collection.UpdateManyAsync(filter, update,options);
        }
        
        public virtual void UpdateOne(Expression<Func<TEntity, bool>> filter, UpdateDefinition<TEntity> update,UpdateOptions options = null)
        {
             Collection.UpdateOne(filter, update,options);
        }

        public virtual async Task UpdateOneAsync(Expression<Func<TEntity, bool>> filter, UpdateDefinition<TEntity> update,UpdateOptions options = null)
        {
            await Collection.UpdateOneAsync(filter, update,options);
        }

        public virtual void Delete(TEntity entity)
        {
            Delete(entity.Id);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
             await Collection.FindOneAndDeleteAsync(filter);
        }

        public virtual void Delete(TPrimaryKey id)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
            Collection.FindOneAndDelete(filter);
        }

        public virtual async Task DeleteAsync(TPrimaryKey id)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
            await Collection.FindOneAndDeleteAsync(filter);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            Collection.DeleteMany(predicate);
        }

        public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            await  Collection.DeleteManyAsync(predicate);
        }

        public virtual void DeleteOne(Expression<Func<TEntity, bool>> predicate)
        {
            Collection.DeleteOne(predicate);
        }

        public virtual async Task DeleteOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            await  Collection.DeleteOneAsync(predicate);
        }


        public virtual long Count()
        {
            return Collection.EstimatedDocumentCount();
        }

        public virtual async Task<long> CountAsync()
        {
            return await Collection.EstimatedDocumentCountAsync();
        }

        public virtual long Count(Expression<Func<TEntity, bool>> predicate)
        {
            return Collection.CountDocuments(predicate);
        }

        public virtual async Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.CountDocumentsAsync(predicate);
        }
    }
}