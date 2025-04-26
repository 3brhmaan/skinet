using Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Services;
public class ResponseCacheService(IConnectionMultiplexer redis) : IResponseCacheService
{
    private readonly IDatabase _database = redis.GetDatabase(1);

    public async Task CacheReponseAsync(string cacheKey , object respone , TimeSpan timeToLive)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var serializedReponse = JsonSerializer.Serialize(respone , options);

        await _database.StringSetAsync(cacheKey , serializedReponse , timeToLive);
    }

    public async Task<string?> GetCacheResponseAsync(string cacheKey)
    {
        var cacheReponse = await _database.StringGetAsync(cacheKey);

        if (cacheReponse.IsNullOrEmpty)
            return null;

        return cacheReponse;
    }

    public async Task RemoveCacheByPattern(string pattern)
    {
        var server = redis.GetServer(
            redis.GetEndPoints().First()
        );

        var keys = server.Keys(database: 1 , pattern: $"*{pattern}*")
            .ToArray();

        if (keys.Length != 0)
        {
            await _database.KeyDeleteAsync(keys);
        }
    }
}
