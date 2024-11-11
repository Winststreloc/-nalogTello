namespace BuildingBlocks.Cache;

public interface IRedisCacheService
{
    /// <summary>
    /// Retrieves a cached item by its key.
    /// </summary>
    /// <typeparam name="T">The type of the cached item.</typeparam>
    /// <param name="key">The key of the cached item.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the cached item if found, otherwise null.</returns>
    Task<T> GetAsync<T>(string key);

    /// <summary>
    /// Sets a cached item with the specified key and value.
    /// </summary>
    /// <typeparam name="T">The type of the cached item.</typeparam>
    /// <param name="key">The key of the cached item.</param>
    /// <param name="value">The value of the cached item.</param>
    /// <param name="expiration">The expiration time for the cached item. If null, the item does not expire.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);

    /// <summary>
    /// Retrieves multiple cached items by their keys.
    /// </summary>
    /// <typeparam name="T">The type of the cached items.</typeparam>
    /// <param name="keys">The keys of the cached items.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a dictionary of keys and their corresponding cached items.</returns>
    Task<IDictionary<string, T>> BulkGetAsync<T>(IEnumerable<string> keys) where T : class;

    /// <summary>
    /// Sets multiple cached items with the specified keys and values.
    /// </summary>
    /// <typeparam name="T">The type of the cached items.</typeparam>
    /// <param name="values">A dictionary of keys and their corresponding values.</param>
    /// <param name="expiration">The expiration time for the cached items. If null, the items do not expire.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task BulkSetAsync<T>(IDictionary<string, T> values, TimeSpan? expiration = null);
 
    /// <summary>
    /// Deletes multiple cached items by their keys.
    /// </summary>
    /// <param name="keys">The keys of the cached items to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task BulkDeleteAsync(List<string> keys);

    /// <summary>
    /// Deletes multiple cached items that match any of the specified patterns.
    /// </summary>
    /// <param name="patterns">A list of patterns to match the keys of the cached items to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task BulkDeleteByPatternAsync(params string[] patterns);

    /// <summary>
    /// Deletes a cached item by its key.
    /// </summary>
    /// <param name="key">The key of the cached item to delete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the item was successfully deleted.</returns>
    Task<bool> DeleteAsync(string key);
}