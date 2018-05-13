using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace ConfigurationReader {
    public interface ICacheService {
        Task<T> GetValue<T>(string applicatonName, string key,
            TimeSpan expirationTime, Func<string, string, Task<T>> valueProvider);
    }

    public class CacheService : ICacheService {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache) {
            _memoryCache = memoryCache;
        }

        public async Task<T> GetValue<T>(string applicatonName, string key,
            TimeSpan expirationTime, Func<string, string, Task<T>> valueProvider) {
            async Task<T> GetValue() {
                return await valueProvider(applicatonName, key);
            }

            var previousCacheKey = $"Previous-{applicatonName}-{key}";

            await _memoryCache.GetOrCreateAsync(previousCacheKey, async entry => {
                entry.SetPriority(CacheItemPriority.NeverRemove);
                entry.SetAbsoluteExpiration(DateTimeOffset.MaxValue);

                return await Task.FromResult(default(T));
            });

            var cacheKey = $"{applicatonName}-{key}";

            var value = await _memoryCache.GetOrCreateAsync(cacheKey, async entry => {
                entry.SetPriority(CacheItemPriority.NeverRemove);
                entry.SetAbsoluteExpiration(expirationTime);

                return await TryGetValueImpl(previousCacheKey, async () => await GetValue());
            });

            return value;
        }

        private async Task<T> TryGetValueImpl<T>(string key, Func<Task<T>> value) {
            try {
                return _memoryCache.Set(key, await value());
            } catch {
                return _memoryCache.Get<T>(key);
            }
        }
    }
}