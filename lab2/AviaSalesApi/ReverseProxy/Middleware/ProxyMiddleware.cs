using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ReverseProxy.Middleware
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class ProxyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HttpClient _httpClient;
        private readonly string[] _hosts;

        public ProxyMiddleware(RequestDelegate next, IOptions<ApiUrls> hosts)
        {
            _next = next;
            _httpClient = new HttpClient();
            _hosts = hosts.Value.Hosts.Split(',');
        }

        public async Task Invoke(HttpContext ctx)
        {
            var origin = $"{ctx.Request.Scheme}://{ctx.Request.Host}{ctx.Request.Path}";
            var request = new HttpRequestMessage(new HttpMethod(ctx.Request.Method), GetRequestUri(ctx.Request));
            request.Headers.Add("Origin", origin);
            SetRequestBody(request, ctx);
            SetRequestHeaders(request, ctx);

            try
            {
                await SendRequestAsync(request, ctx);
            }
            catch (HttpRequestException e)
            {
                ctx.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            }
        }

        private async Task SendRequestAsync(HttpRequestMessage request, HttpContext ctx)
        {
            using var response = await _httpClient.SendAsync(request, 
                HttpCompletionOption.ResponseHeadersRead, ctx.RequestAborted);
            ctx.Response.StatusCode = (int) response.StatusCode;
            ctx.Response.ContentType = response.Content?.Headers.ContentType?.MediaType;
            foreach (var (key, value) in ctx.Response.Headers)
            {
                ctx.Response.Headers[key] = value.ToArray();
            }

            ctx.Response.Headers.Remove("transfer-encoding");

            if (response.Content != null)
            {
                foreach (var (key, value) in response.Content.Headers)
                {
                    ctx.Response.Headers[key] = value.ToArray();
                }

                await response.Content.CopyToAsync(ctx.Response.Body);
            }
        }

        private Uri GetRequestUri(HttpRequest request)
        {
            var uriString = $"{_hosts[0]}{request.Path}{request.QueryString}";
            return new Uri(uriString);
        }

        private static void SetRequestBody(HttpRequestMessage message, HttpContext ctx)
        {
            var method = ctx.Request.Method;
            if (HttpMethods.IsGet(method) || HttpMethods.IsDelete(method) || HttpMethods.IsHead(method) ||
                HttpMethods.IsOptions(method)) return;
           
            var content = new StreamContent(ctx.Request.Body);
            message.Content = content;
        }

        private static void SetRequestHeaders(HttpRequestMessage message, HttpContext ctx)
        {
            foreach (var (key, value) in ctx.Request.Headers)
            {
                if (! message.Headers.TryAddWithoutValidation(key, value.ToArray()))
                {
                    message.Content?.Headers.TryAddWithoutValidation(key, value.ToArray());
                }
            }
        }
    }
}