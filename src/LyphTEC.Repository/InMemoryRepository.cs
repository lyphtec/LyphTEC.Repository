using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LyphTEC.Repository.Extensions;

namespace LyphTEC.Repository
{
    /// <summary>
    /// A thread-safe in-memory repository implementation
    /// </summary>
    /// <remarks>We are assuming the TEntity.Id field to be an integer</remarks>
    /// <typeparam name="TEntity"></typeparam>
    [Export(typeof(IRepository<>))]
    [Export(typeof(IRepositoryAsync<>))]
    [Shared]
    public class InMemoryRepository<TEntity> : IRepository<TEntity>, IRepositoryAsync<TEntity>
        where TEntity : class, IEntity
    {
        private readonly ConcurrentDictionary<int, object> _repo = InMemoryDatastore.GetCollection<TEntity>();

        private int GetNextKey()
        {
            var nextKey = 1;

            if (!_repo.IsEmpty)
                nextKey = _repo.Keys.Max() + 1;

            return nextKey;
        }

        #region IRepository<TEntity> Members

        public TEntity Save(TEntity entity)
        {
            var key = entity.Id == null ? default(int) : (int)entity.Id;

            if (key == default(int))
            {
                key = GetNextKey();
                entity.Id = key;
            }
            
            return _repo.AddOrUpdate(key, entity, (i, o) => entity) as TEntity;
        }

        public void SaveAll(IEnumerable<TEntity> entities)
        {
            entities.ForEach(x => Save(x));
        }

        public void Remove(TEntity entity)
        {
            Remove(entity.Id);
        }

        public void Remove(object id)
        {
            if (id == null)
                return;

            var key = (int)id;

            object removed;
            _repo.TryRemove(key, out removed);
        }

        public void RemoveByIds(System.Collections.IEnumerable ids)
        {
            var keys = ids.Cast<object>().Select(x => (int) x);

            keys.ForEach(x => Remove(x));
        }

        public void RemoveAll()
        {
            _repo.Clear();
        }

        public TEntity One(object id)
        {
            if (id == null || (int) id < 1)
                return null;

            var key = (int) id;

            object entity;
            return _repo.TryGetValue(key, out entity) ? entity as TEntity : null;
        }

        public TEntity One(Expression<Func<TEntity, bool>> predicate)
        {
            return _repo.Values.Cast<TEntity>().SingleOrDefault(predicate.Compile());
        }

        public IQueryable<TEntity> All(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null ? _repo.Values.Cast<TEntity>().AsQueryable() : _repo.Values.Cast<TEntity>().Where(predicate.Compile()).AsQueryable();
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null ? !_repo.IsEmpty : _repo.Values.Cast<TEntity>().Any(predicate.Compile());
        }

        public int Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null ? _repo.Count : _repo.Values.Cast<TEntity>().Count(predicate.Compile());
        }

        #endregion

        #region IRepositoryAsync<TEntity> Members

        public Task<TEntity> SaveAsync(TEntity entity)
        {
            Debug.WriteLine("SaveAsync() : ThreadId - {0}", System.Threading.Thread.CurrentThread.ManagedThreadId);
            return Task.Run(() => Save(entity));
        }

        public Task<bool> SaveAllAsync(IEnumerable<TEntity> entities)
        {
            return Task.Run(() =>
                                {
                                    SaveAll(entities);
                                    return true;
                                });
        }

        public Task<bool> RemoveAsync(TEntity entity)
        {
            return Task.Run(() =>
                                {
                                    Remove(entity);
                                    return true;
                                });
        }

        public Task<bool> RemoveAsync(object id)
        {
            return Task.Run(() =>
                                {
                                    Remove(id);
                                    return true;
                                });
        }
        
        public Task<bool> RemoveByIdsAsync(System.Collections.IEnumerable ids)
        {
            return Task.Run(() =>
                                {
                                    RemoveByIds(ids);
                                    return true;
                                });
        }

        public Task<bool> RemoveAllAsync()
        {
            return Task.Run(() =>
                                {
                                    RemoveAll();
                                    return true;
                                });
        }

        public Task<TEntity> OneAsync(object id)
        {
            return Task.Run(() => One(id));
        }

        public Task<TEntity> OneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.Run(() => One(predicate));
        }

        public Task<IQueryable<TEntity>> AllAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return Task.Run(() => All(predicate));
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return Task.Run(() => Any(predicate));
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return Task.Run(() => Count(predicate));
        }

        #endregion
    }
}
