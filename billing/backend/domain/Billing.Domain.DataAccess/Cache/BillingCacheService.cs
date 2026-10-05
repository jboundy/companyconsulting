using Microsoft.Extensions.Caching.Memory;

namespace Billing.Domain.DataAccess.Cache
{
    public class BillingCacheService(IMemoryCache cache) : IBillingCacheService
    {
        private readonly IMemoryCache _cache = cache;
        public T Create<T>(string key, T cacheEntry, TimeSpan absoluteExpiration)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(absoluteExpiration);
            _cache.Set(key, cacheEntry, cacheEntryOptions);

            return cacheEntry;
        }

        public T CreateWithCallbackOptions<T>(string key, T cacheEntry, TimeSpan absoluteExpiration, PostEvictionDelegate callback)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(absoluteExpiration)
                .RegisterPostEvictionCallback(callback);
            _cache.Set(key, cacheEntry, cacheEntryOptions);

            return cacheEntry;
        }

        public T? Get<T>(string key)
        {
            _cache.TryGetValue(key, out T? cacheEntry);
            return cacheEntry;
        }

        public void Remove(string key)
        {
            if (_cache.TryGetValue(key, out _))
            {
                _cache.Remove(key);
            }
        }
    }
}