using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReverseProxy.Caching;
using ReverseProxy.Services.Impl;
using ReverseProxy.Services.Interfaces;

namespace ReverseProxy.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRedisCaching(this IServiceCollection services, IConfiguration configuration)
        {
            var redisCacheSettings = new RedisCacheSettings();
            configuration.GetSection(nameof(RedisCacheSettings)).Bind(redisCacheSettings);
            services.AddSingleton(redisCacheSettings);

            if (!redisCacheSettings.IsEnabled)
            {
                return services;
            }

            services.AddStackExchangeRedisCache(opt => opt.Configuration = redisCacheSettings.ConnectionsString);
            
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            
            return services;
        }
    }
}