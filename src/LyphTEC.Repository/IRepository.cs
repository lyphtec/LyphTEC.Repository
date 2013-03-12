using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LyphTEC.Repository
{
    /// <summary>
    /// A simple repository interface
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
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
    }
}
