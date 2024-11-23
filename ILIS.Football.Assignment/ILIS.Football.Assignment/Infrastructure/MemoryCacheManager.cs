using Microsoft.Extensions.Caching.Memory;

namespace ILIS.Football.Assignment.Infrastructure
{
    public interface IMemoryCacheManager
    {
        T Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan absoluteExpirationRelativeToNow);
        void Remove(string key);
    }

    public class MemoryCacheManager : IMemoryCacheManager
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T Get<T>(string key)
        {
            _memoryCache.TryGetValue(key, out T value);
            return value;
        }

        public void Set<T>(string key, T value, TimeSpan absoluteExpirationRelativeToNow)
        {
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow
            };
            _memoryCache.Set(key, value, cacheOptions);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}