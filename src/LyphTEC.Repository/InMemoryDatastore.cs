using System.Collections.Concurrent;

namespace LyphTEC.Repository;

/// <summary>
/// Used by InMemoryRepository to store data
/// </summary>
internal static class InMemoryDatastore
{
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<int, object>> Store = new();

    public static ConcurrentDictionary<int, object> GetCollection<TEntity>() where TEntity : class, IEntity
    {
        return Store.GetOrAdd(typeof (TEntity).FullName, new ConcurrentDictionary<int, object>());
    }
}
