using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace ReverseProxy.Middleware
{
    public class ProxyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HttpClient _httpClient = new HttpClient();

        public ProxyMiddleware(RequestDelegate next)
        {
            _next = next;
        }
    }
}