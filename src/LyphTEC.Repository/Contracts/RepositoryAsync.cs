using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyphTEC.Repository.Contracts
{
    [ContractClassFor(typeof(IRepositoryAsync<>))]
    internal abstract class RepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class, IEntity
    {
        #region IRepositoryAsyncTEntity> Members

        public Task<TEntity> SaveAsync(TEntity entity)
        {
            ContractValidation.IsNotNull(entity);
            return null;
        }

        public Task<bool> SaveAllAsync(IEnumerable<TEntity> entities)
        {
            Contract.Requires<ArgumentNullException>(entities != null && entities.Any());
            return null;
        }

        public Task<bool> RemoveAsync(TEntity entity)
        {
            ContractValidation.IsNotNull(entity);
            return null;
        }

        public Task<bool> RemoveAsync(object id)
        {
            ContractValidation.IsNotNull(id);
            return null;
        }

        public Task<bool> RemoveByIdsAsync(System.Collections.IEnumerable ids)
        {
            ContractValidation.IsNotNull(ids);
            return null;
        }

        public Task<bool> RemoveAllAsync()
        {
            return null;
        }

        public Task<TEntity> OneAsync(object id)
        {
            ContractValidation.IsNotNull(id);
            return null;
        }

        public Task<TEntity> OneAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return null;
        }

        public Task<IQueryable<TEntity>> AllAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate = null)
        {
            return null;
        }

        public Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate = null)
        {
            return null;
        }

        public Task<int> CountAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate = null)
        {
            return null;
        }

        #endregion
    }
}
