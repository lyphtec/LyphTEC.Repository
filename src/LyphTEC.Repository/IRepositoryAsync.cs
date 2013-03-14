using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LyphTEC.Repository
{
    /// <summary>
    /// Async repository interface
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    [ContractClass(typeof(Contracts.RepositoryAsync<>))]
    public interface IRepositoryAsync<TEntity> where TEntity : class, IEntity
    {
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
    }
}
