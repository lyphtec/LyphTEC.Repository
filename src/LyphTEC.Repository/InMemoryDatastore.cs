using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyphTEC.Repository
{
    /// <summary>
    /// Used by InMemoryRepository to store data
    /// </summary>
    internal static class InMemoryDatastore
    {
        private static readonly ConcurrentDictionary<string, ConcurrentDictionary<int, object>> Store = new ConcurrentDictionary<string, ConcurrentDictionary<int, object>>();

        public static ConcurrentDictionary<int, object> GetCollection<TEntity>() where TEntity : class, IEntity
        {
            return Store.GetOrAdd(typeof (TEntity).FullName, new ConcurrentDictionary<int, object>());
        }
    }
}
