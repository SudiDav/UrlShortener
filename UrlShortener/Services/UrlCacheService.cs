using Microsoft.Extensions.Caching.Memory;

namespace UrlShortener.Services;

public class UrlCacheService(IMemoryCache cache)
{
    public void CacheUrl(string shortId, string originalUrl) 
        => cache.Set(shortId, originalUrl, TimeSpan.FromHours(24));

    public string GetCachedUrl(string shortId) 
        => cache.Get<string>(shortId);
}