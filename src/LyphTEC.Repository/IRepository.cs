using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LyphTEC.Repository
{
    /// <summary>
    /// A simple repository interface
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    [ContractClass(typeof(Contracts.Repository<>))]
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        #region Sync Methods
        TEntity Save(TEntity entity);
        void SaveAll(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void Remove(object id);
        void RemoveByIds(System.Collections.IEnumerable ids);
        void RemoveAll();

        TEntity One(object id);
        TEntity One(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> All(Expression<Func<TEntity, bool>> predicate = null);

        bool Any(Expression<Func<TEntity, bool>> predicate = null);

        int Count(Expression<Func<TEntity, bool>> predicate = null);
        #endregion

        #region Async Methods
        Task<TEntity> SaveAsync(TEntity entity);
        Task<bool> SaveAllAsync(IEnumerable<TEntity> entities);

        Task<bool> RemoveAsync(TEntity entity);
        Task<bool> RemoveAsync(object id);
        Task<bool> RemoveByIdsAsync(System.Collections.IEnumerable ids);
        Task<bool> RemoveAllAsync();

        Task<TEntity> OneAsync(object id);
        Task<TEntity> OneAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IQueryable<TEntity>> AllAsync(Expression<Func<TEntity, bool>> predicate = null);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null);
        #endregion
    }
}
