using System.Text.Json;
using BuildingBlocks.Cache;
using StackExchange.Redis;

namespace BuildingBlocks;

public class RedisCacheService : IRedisCacheService
{
private readonly IDatabase _database;
    private readonly IConnectionMultiplexer _multiplexer;

    private readonly RedisOptions _redisOptions;

    public RedisCacheService(IConnectionMultiplexer multiplexer, RedisOptions redisOptions)
    {
        _multiplexer = multiplexer;
        _redisOptions = redisOptions;
        _database = multiplexer.GetDatabase();
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var serializedValue = JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, serializedValue, expiration ?? _redisOptions.DefaultExpiration);
    }

    public async Task<IDictionary<string, T>> BulkGetAsync<T>(IEnumerable<string> keys)
        where T : class
    {
        var redisKeys = keys.Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct().Select(key => (RedisKey)key).ToArray();
        var chunks = redisKeys.Chunk(_redisOptions.DefaultChunkSize);

        List<RedisValue> results = new List<RedisValue>();
        foreach (var chunk in chunks)
        {
            results.AddRange(await _database.StringGetAsync(chunk));
            ;
        }

        return redisKeys
            .Select((key, index) => new { key, result = results[index] })
            .ToDictionary(
                x => (string)x.key,
                x => x.result.HasValue
                    ? JsonSerializer.Deserialize<T>(x.result, _redisOptions.JsonSerializerOptions)
                    : null
            );
    }

    public async Task BulkSetAsync<T>(IDictionary<string, T> values, TimeSpan? expiration = null)
    {
        using var ms = new MemoryStream();
        var list = new List<Task<bool>>();

        var chunks = values.Chunk(_redisOptions.DefaultChunkSize / 10);
        foreach (var chunk in chunks)
        {
            foreach (var item in chunk)
            {
                ms.Position = 0;
                ms.SetLength(0);
                JsonSerializer.Serialize(ms, item.Value, _redisOptions.JsonSerializerOptions);
                list.Add(
                    _database.StringSetAsync(item.Key, ms.ToArray(), expiration ?? _redisOptions.DefaultExpiration));
            }
        }

        await Task.WhenAll(list);
    }

    public async Task BulkDeleteAsync(List<string> keys)
    {
        await _database.KeyDeleteAsync(keys.Select(x => (RedisKey)x).ToArray());
    }

    public async Task<bool> DeleteAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }

    public async Task BulkDeleteByPatternAsync(params string[] patterns)
    {
        foreach (var ep in _multiplexer.GetEndPoints())
        {
            var server = _multiplexer.GetServer(ep);
            await DeleteKeysByPatternsAsync(server, patterns);
        }
    }
    private async Task DeleteKeysByPatternsAsync(IServer server, string[] patterns)
    {
        foreach (var pattern in patterns)
        {
            var matchingKeys = server.Keys(
                pattern: pattern
            ).ToArray();
            
            await _database.KeyDeleteAsync(matchingKeys);
        }
    }
}