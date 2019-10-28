using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mongo.Demo.Core.Repository;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongo.Demo.Core
{
    /// <inheritdoc />
    /// <summary>
    ///     A shortcut of <see cref="T:Mongo.Demo.Core.IMongoRepository`2" /> for most used primary key type (
    ///     <see cref="T:MongoDB.Bson.ObjectId" />).
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public interface IMongoRepository<TEntity> : IMongoRepository<TEntity, ObjectId>
        where TEntity : class, IEntity<ObjectId>
    {
    }

    /// <inheritdoc />
    /// <summary>
    ///     This interface is implemented by all mongo repositories to ensure implementation of fixed methods.
    /// </summary>
    /// <typeparam name="TEntity">Main Entity type this mongo repository works on</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
    public interface IMongoRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        IMongoCollection<TEntity> Collection { get; }

        // <summary>
        ///     获取IQueryable,使用IQueryable执行查询
        /// </summary>
        /// <returns>IQueryable to be used to select entities from database</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        ///     获取所有实体
        /// </summary>
        /// <returns>List of all entities</returns>
        IList<TEntity> GetAllList();

        /// <summary>
        ///     使用指定的<paramref name="predicate"/>获取实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        IList<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate, FindOptions options = null);

        IList<TEntity> GetAllList(FilterDefinition<TEntity> filter, FindOptions options = null);

        Task<IList<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate, FindOptions options = null);

        Task<IList<TEntity>> GetAllListAsync(FilterDefinition<TEntity> filter, FindOptions options = null);

        T Query<T>(Func<IQueryable<TEntity>, T> queryMethod);

        /// <summary>
        ///     根据主键获取实体
        /// </summary>
        /// <param name="id">primary key</param>
        /// <returns>Entity</returns>
        /// <exception cref="EntityNotFoundException">
        ///    未找到与<paramref name="id"/>匹配的实体
        /// </exception>
        TEntity Get(TPrimaryKey id);

        /// <summary>
        ///     根据主键获取实体
        /// </summary>
        /// <param name="id">primary key</param>
        /// <returns>Entity</returns>
        /// <exception cref="EntityNotFoundException">
        ///    未找到与<paramref name="id"/>匹配的实体
        /// </exception>
        Task<TEntity> GetAsync(TPrimaryKey id);

        /// <summary>
        ///     根据主键获取实体,如果没有返回null
        /// </summary>
        /// <param name="id">primary key</param>
        /// <returns>TEntity or null</returns>
        TEntity FirstOrDefault(TPrimaryKey id);

        /// <summary>
        ///     根据主键获取实体,如果没有返回null
        /// </summary>
        /// <param name="id">primary key</param>
        /// <returns>TEntity or null</returns>
        Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id);

        /// <summary>
        ///    根据指定条件获取第一个实体或null
        /// </summary>
        /// <param name="predicate">Predicate to filter entities</param>
        /// <returns>TEntity or null</returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///    根据指定条件获取第一个实体或null
        /// </summary>
        /// <param name="predicate">Predicate to filter entities</param>
        /// <returns>TEntity or null</returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Aggregate Query
        /// </summary>
        /// <param name="pipelineDefinition"></param>
        /// <param name="options"></param>
        /// <returns>List of TOutEntity</returns>
        IList<TOutEntity> Aggregate<TOutEntity>(PipelineDefinition<TEntity, TOutEntity> pipelineDefinition,
            AggregateOptions options = null);

        /// <summary>插入一个新实体</summary>
        /// <param name="entity">Inserted entity</param>
        /// <param name="options"></param>
        /// <returns>New inserted entity</returns>
        TEntity Insert(TEntity entity, InsertOneOptions options = null);

        /// <summary>inserts a new entity</summary>
        /// <param name="entity">Inserted entity</param>
        /// <param name="options"></param>
        /// <returns>New inserted entity</returns>
        Task<TEntity> InsertAsync(TEntity entity, InsertOneOptions options = null);

        /// <summary>
        ///     inserts many new entity
        /// </summary>
        /// <param name="entities">list of entity</param>
        /// <param name="options"></param>
        void InsertMany(IEnumerable<TEntity> entities, InsertManyOptions options = null);

        /// <summary>
        ///     inserts many new entity
        /// </summary>
        /// <param name="entities">list of entity</param>
        /// <param name="options"></param>
        Task InsertManyAsync(IEnumerable<TEntity> entities, InsertManyOptions options = null);

        /// <summary>
        ///     replace a new entity used <paramref name="filter"/> filter
        /// </summary>
        /// <param name="filter">Predicate to filter entity</param>
        /// <param name="entity">new entity</param>
        /// <param name="options"></param>
        void ReplaceOne(FilterDefinition<TEntity> filter, TEntity entity, UpdateOptions options = null);

        /// <summary>
        ///     replace a new entity used <paramref name="filter"/> filter
        /// </summary>
        /// <param name="filter">Predicate to filter entity</param>
        /// <param name="entity">new entity</param>
        /// <param name="options"></param>
        Task ReplaceOneAsync(FilterDefinition<TEntity> filter, TEntity entity, UpdateOptions options = null);

        /// <summary>
        ///     Inserts a new entity and gets it's Id.
        ///     It may require to save current unit of work
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="options"></param>
        /// <returns>Id of the entity</returns>
        TPrimaryKey InsertAndGetId(TEntity entity, InsertOneOptions options = null);

        /// <summary>
        ///     Inserts a new entity and gets it's Id.
        ///     It may require to save current unit of work
        ///     to be able to retrieve id.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="options"></param>
        /// <returns>Id of the entity</returns>
        Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity, InsertOneOptions options = null);

        /// <summary>Updates an existing entity.</summary>
        /// <param name="entity">Entity</param>
        /// <param name="options"></param>
        TEntity Update(TEntity entity, UpdateOptions options = null);

        /// <summary>Updates an existing entity.</summary>
        /// <param name="entity">Entity</param>
        /// <param name="options"></param>
        Task<TEntity> UpdateAsync(TEntity entity, UpdateOptions options = null);

        /// <summary>Updates an existing entity.</summary>
        /// <param name="id">Id of the entity</param>
        /// <param name="updateAction">Action that can be used to change values of the entity</param>
        /// <returns>Updated entity</returns>
        TEntity Update(TPrimaryKey id, Action<TEntity> updateAction);

        /// <summary>Updates an existing entity.</summary>
        /// <param name="id">Id of the entity</param>
        /// <param name="updateAction">Action that can be used to change values of the entity</param>
        /// <param name="options"></param>
        /// <returns>Updated entity</returns>
        Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction, UpdateOptions options = null);

        /// <summary>
        ///     updates all existing entity used filter <paramref name="filter"/>
        /// </summary>
        /// <param name="filter">update filter</param>
        /// <param name="update">update field</param>
        void Update(Expression<Func<TEntity, bool>> filter, UpdateDefinition<TEntity> update,
            UpdateOptions options = null);

        /// <summary>
        ///     updates all existing entity used filter <paramref name="filter"/>
        /// </summary>
        /// <param name="filter">update filter</param>
        /// <param name="update">update field</param>
        Task UpdateAsync(Expression<Func<TEntity, bool>> filter, UpdateDefinition<TEntity> update,
            UpdateOptions options = null);

        /// <summary>
        ///     updates a existing entity
        /// </summary>
        /// <param name="filter">update filter</param>
        /// <param name="update">update field</param>
        void UpdateOne(Expression<Func<TEntity, bool>> filter, UpdateDefinition<TEntity> update,
            UpdateOptions options = null);

        /// <summary>
        ///     updates a existing entity
        /// </summary>
        /// <param name="filter">update filter</param>
        /// <param name="update">update field</param>
        /// <param name="options"></param>
        Task UpdateOneAsync(Expression<Func<TEntity, bool>> filter, UpdateDefinition<TEntity> update,
            UpdateOptions options = null);

        /// <summary>Deletes an entity.</summary>
        /// <param name="entity">Entity to be deleted</param>
        void Delete(TEntity entity);

        /// <summary>Deletes an entity.</summary>
        /// <param name="entity">Entity to be deleted</param>
        Task DeleteAsync(TEntity entity);

        /// <summary>Deletes an entity by primary key.</summary>
        /// <param name="id">Primary key of the entity</param>
        void Delete(TPrimaryKey id);

        /// <summary>Deletes an entity by primary key.</summary>
        /// <param name="id">Primary key of the entity</param>
        Task DeleteAsync(TPrimaryKey id);

        /// <summary>
        ///     Deletes many entities by function.
        ///     Notice that: All entities fits to given predicate are retrieved and deleted.
        ///     This may cause major performance problems if there are too many entities with
        ///     given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        void Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Deletes many entities by function.
        ///     Notice that: All entities fits to given predicate are retrieved and deleted.
        ///     This may cause major performance problems if there are too many entities with
        ///     given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     delete a existing entity
        /// </summary>
        /// <param name="filter">delete filter</param>
        void DeleteOne(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        ///     delete a existing entity
        /// </summary>
        /// <param name="filter">delete filter</param>
        Task DeleteOneAsync(Expression<Func<TEntity, bool>> filter);

        /// <summary>Gets count of all entities in this repository.</summary>
        /// <returns>Count of entities</returns>
        long Count();

        /// <summary>Gets count of all entities in this repository.</summary>
        /// <returns>Count of entities</returns>
        Task<long> CountAsync();

        /// <summary>
        ///     Gets count of all entities in this repository based on given <paramref name="predicate" />.
        /// </summary>
        /// <param name="predicate">A method to filter count</param>
        /// <returns>Count of entities</returns>
        long Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Gets count of all entities in this repository based on given <paramref name="predicate" />.
        /// </summary>
        /// <param name="predicate">A method to filter count</param>
        /// <returns>Count of entities</returns>
        Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate);
    }
}