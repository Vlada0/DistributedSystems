using System;
using System.Threading.Tasks;

namespace ReverseProxy.Services.Interfaces
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string key, string response, TimeSpan ttl);
        Task<string> GetCachedResponseAsync(string key);
    }
}