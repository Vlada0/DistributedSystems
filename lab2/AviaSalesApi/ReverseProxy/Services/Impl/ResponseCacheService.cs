using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ReverseProxy.Services.Interfaces;

namespace ReverseProxy.Services.Impl
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache _distributedCache;

        public ResponseCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task CacheResponseAsync(string key, string response, TimeSpan ttl)
        {
            if (response == null) return;

            var serializedContent = JsonConvert.SerializeObject(response);
            await _distributedCache.SetStringAsync(key, response, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = ttl
            });
        }

        public async Task<string> GetCachedResponseAsync(string key)
        {
            var cachedResponse = await _distributedCache.GetStringAsync(key);

            return string.IsNullOrEmpty(cachedResponse) ? null : cachedResponse;
        }
    }
}