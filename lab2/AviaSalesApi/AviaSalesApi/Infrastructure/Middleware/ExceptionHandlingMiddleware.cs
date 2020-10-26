using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using AviaSalesApi.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AviaSalesApi.Infrastructure.Middleware
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _nxt;
        private const int ProxyPort = 4444;

        public ExceptionHandlingMiddleware(RequestDelegate nxt)
        {
            _nxt = nxt;
        }
        
        public async Task Invoke(HttpContext ctx)
        {
            string origin = ctx.Request.Headers["Origin"];
            int? port = null;
            if (! string.IsNullOrWhiteSpace(origin))
            {
                var uri = new Uri(origin);
                port = uri.Port;
            }

            if (port == null || port != ProxyPort)
            {
                var response = ctx.Response;
                response.ContentType = "application/json";
                response.StatusCode = StatusCodes.Status403Forbidden;
                
                await ctx.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    Error = "Host is not allowed."
                }));
                return;
            }
            
            try
            {
                await _nxt.Invoke(ctx);
            }
            catch (EntityNotFoundException e)
            {
                await HandleExceptionAsync(ctx, e, HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(ctx, e, HttpStatusCode.InternalServerError);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext ctx, Exception ex, HttpStatusCode statusCode)
        {
            var response = ctx.Response;
            response.ContentType = Consts.AppProblemPlusJsonContentType;
            response.StatusCode = (int) statusCode;
            await response.WriteAsync(JsonConvert.SerializeObject(new
            {
                StatusCode = (int) statusCode,
                Error = ex.Message
            }));
        }
    }
}