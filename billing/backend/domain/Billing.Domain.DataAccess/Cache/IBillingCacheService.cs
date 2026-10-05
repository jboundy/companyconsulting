using Microsoft.Extensions.Caching.Memory;

namespace Billing.Domain.DataAccess.Cache
{
    public interface IBillingCacheService
    {
        T? Get<T>(string key);

        T Create<T>(string key, T cacheEntry, TimeSpan absoluteExpiration);

        T CreateWithCallbackOptions<T>(string key, T cacheEntry, TimeSpan absoluteExpiration, PostEvictionDelegate callback);

        void Remove(string key);
    }
}