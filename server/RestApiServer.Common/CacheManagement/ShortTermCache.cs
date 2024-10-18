using RestApiServer.Common.Config;
using Microsoft.Extensions.Caching.Memory;

namespace RestApiServer.Core.CacheManagement
{
    public class ShortTermCache
    {
        private static readonly MemoryCache _ShortTermCache = new(new MemoryCacheOptions());
        public static T? TryGetFromCache<T>(string cachedDataId)
        {
            var type = typeof(T);
            var cachedData = _ShortTermCache.Get<T>(GetCacheKey(cachedDataId, type));
            return cachedData;
        }

        public static void AddToCache<T>(string cachedDataId, T item)
        {
            AddToCache(cachedDataId, item, ConfigurationLoader.GetConfigValueAsInt(EnvironmentVariable.ShortTermCacheExpirationSecs));
        }

        public static void AddToCache<T>(string cachedDataId, T item, int expirationSecs)
        {
            var type = typeof(T);
            _ShortTermCache.Set(GetCacheKey(cachedDataId, type), item, TimeSpan.FromSeconds(expirationSecs));
        }

        public static void RemoveFromCache(string cachedDataId, Type type)
        {
            _ShortTermCache.Remove(GetCacheKey(cachedDataId, type));
        }

        private static string GetCacheKey(string cachedDataId, Type type)
        {
            return $"{type}#{cachedDataId}";
        }
    }
}