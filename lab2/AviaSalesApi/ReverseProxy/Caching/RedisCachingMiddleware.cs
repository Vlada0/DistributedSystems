using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ReverseProxy.Services.Interfaces;
// ReSharper disable All

namespace ReverseProxy.Caching
{
    public class RedisCachingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int _timeToLiveSec;

        public RedisCachingMiddleware(RequestDelegate next)
        {
            _next = next;
            _timeToLiveSec = 60;
        }

        public async Task Invoke(HttpContext ctx)
        {
            if (!HttpMethods.IsGet(ctx.Request.Method))
            {
                await _next.Invoke(ctx);
                return;
            }

            var cacheSetting = ctx.RequestServices.GetRequiredService<RedisCacheSettings>();

            if (!cacheSetting.IsEnabled)
            {
                await _next.Invoke(ctx);
                return;
            }

            var cacheService = ctx.RequestServices.GetRequiredService<IResponseCacheService>();

            var cacheKey = GenerateCacheKeyFromRequestUrl(ctx.Request);

            var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                ctx.Response.StatusCode = StatusCodes.Status200OK;
                var accept = ctx.Request.Headers?["Accept"].ToArray();
                ctx.Response.ContentType = ctx.Request.Headers?["Accept"] ?? "application/json";
                await ctx.Response.WriteAsync(cachedResponse);
                return;
            }
            
            var responseBody = ctx.Response.Body;

            try
            {
                using (var memStream = new MemoryStream())
                {
                    ctx.Response.Body = memStream;

                    await _next.Invoke(ctx);

                    memStream.Position = 0;

                    using (var reader = new StreamReader(memStream))
                    {
                        var responseString = await reader.ReadToEndAsync();

                        memStream.Position = 0;
                        await memStream.CopyToAsync(responseBody);

                        if (((int) ctx.Response.StatusCode) / 100 == 2)
                        {
                            await cacheService.CacheResponseAsync(cacheKey, responseString, TimeSpan.FromSeconds( _timeToLiveSec));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                ctx.Response.Body = responseBody;
            }
        }

        private static string GenerateCacheKeyFromRequestUrl(HttpRequest request)
        {
            var acceptContentType = request.Headers?["Accept"] ?? "application/json";
            var keyBuilder = new StringBuilder($"{request.Path}");

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            keyBuilder.Append($"|Accept-{acceptContentType}");

            return keyBuilder.ToString();
        }
    }
}