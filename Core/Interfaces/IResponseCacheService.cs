namespace Core.Interfaces;
public interface IResponseCacheService
{
    Task CacheReponseAsync(string cacheKey , object respone , TimeSpan timeToLive);
    Task<string?> GetCacheResponseAsync(string cacheKey);
    Task RemoveCacheByPattern(string pattern);
}
