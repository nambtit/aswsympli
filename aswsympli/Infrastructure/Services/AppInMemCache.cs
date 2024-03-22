using System.Runtime.Caching;
using Application.Abstraction;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class AppInMemCache : IAppDataCache
    {
        private readonly MemoryCache _cache;
        private readonly ILogger<AppInMemCache> _logger;
        private readonly int _cacheDurationMins;

        public AppInMemCache(ILogger<AppInMemCache> logger, IApplicationConfig applicationConfig)
        {
            _cacheDurationMins = applicationConfig.CacheAbsExpiryMinutes;
            _cache = new MemoryCache(nameof(AppInMemCache));
            _logger = logger;
        }

        public bool TryGet<T>(string key, out T value)
        {
            value = default;
            var data = _cache.Get(key);

            if (data == null)
            {
                _logger.LogDebug($"Cache miss for key {key}");
                return false;
            }

            try
            {
                value = (T)data;
                _logger.LogDebug($"Cache hit for key {key}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to cast cache data to type {typeof(T)}. Error: {ex}");
                return false;
            }
        }

        public bool TrySet<T>(string key, T value)
        {
            try
            {
                var policy = new CacheItemPolicy
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(_cacheDurationMins))
                };

                _cache.Set(key, value, policy);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to set cache item. Error: {ex}");
                return false;
            }
        }
    }
}
