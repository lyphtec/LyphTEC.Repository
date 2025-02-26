using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LyphTEC.Repository;

/// <summary>
/// A simple repository interface
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IRepository<TEntity> where TEntity : class, IEntity
{
    #region Sync Methods
    /// <summary>
    /// Saves the entity. Will be added to repository if entity ID is null, otherwise entity is updated.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>The saved entity. Added entity will have ID.</returns>
    TEntity Save(TEntity entity);

    /// <summary>
    /// Saves the list of entities.
    /// </summary>
    /// <param name="entities"></param>
    void Save(IEnumerable<TEntity> entities);

    /// <summary>
    /// Removes entity from repo.
    /// </summary>
    /// <param name="entity"></param>
    void Remove(TEntity entity);

    /// <summary>
    /// Removes entity from repo with specified ID.
    /// </summary>
    /// <param name="id"></param>
    void Remove(object id);

    /// <summary>
    /// Removes entities matching specified IDs.
    /// </summary>
    /// <param name="ids"></param>
    void RemoveByIds(System.Collections.IEnumerable ids);

    /// <summary>
    /// Removes all entities.
    /// </summary>
    void RemoveAll();

    /// <summary>
    /// Gets entity matching ID, or null if not found.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    TEntity Get(object id);

    /// <summary>
    /// Gets first entity matching predicate, or null if not found.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    TEntity Get(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Returns list of entities matching predicate.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> predicate = null);

    /// <summary>
    /// Returns boolean if repo has any entities matching predicate.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    bool Any(Expression<Func<TEntity, bool>> predicate = null);

    /// <summary>
    /// Returns count of entities matching predicate.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    int Count(Expression<Func<TEntity, bool>> predicate = null);
    #endregion

    #region Async Methods
    /// <summary>
    /// Saves the entity asynchronously. Will be added to repository if entity ID is null, otherwise entity is updated.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity> SaveAsync(TEntity entity);
    
    /// <summary>
    /// Saves the list of entities asynchronously.
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    Task<bool> SaveAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// Removes entity from repo asynchronously.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<bool> RemoveAsync(TEntity entity);

    /// <summary>
    /// Removes entity with specified ID from repo asynchronously.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True if successful.</returns>
    Task<bool> RemoveAsync(object id);

    /// <summary>
    /// Removes entities matching list of IDs from repo asynchronously.
    /// </summary>
    /// <param name="ids"></param>
    /// <returns>True if successful.</returns>
    Task<bool> RemoveByIdsAsync(System.Collections.IEnumerable ids);

    /// <summary>
    /// Removes all entities from repo asynchronously.
    /// </summary>
    /// <returns></returns>
    Task<bool> RemoveAllAsync();

    /// <summary>
    /// Gets entity matching ID, or null if not found asynchronously.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TEntity> GetAsync(object id);

    /// <summary>
    /// Gets first entity matching predicate, or null if not found asynchronously.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Returns list of entities matching predicate asynchronously.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> predicate = null);

    /// <summary>
    /// Returns boolean if repo has any entities matching predicate asynchronously.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null);

    /// <summary>
    /// Returns count of entities matching predicate asynchronously.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null);
    #endregion
}

public static class RepositoryExtensions
{
    /// <summary>
    /// Returns all records from the repo.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="repository"></param>
    /// <returns></returns>
    public static IEnumerable<TEntity> All<TEntity>(this IRepository<TEntity> repository) where TEntity : class, IEntity 
        => repository.Query(_ => true);

    /// <summary>
    /// Returns all records from the repo asynchronously.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="repository"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<TEntity>> AllAsync<TEntity>(this IRepository<TEntity> repository) where TEntity : class, IEntity 
        => await repository.QueryAsync(_ => true);
}
