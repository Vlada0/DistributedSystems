using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using AviaSalesApiCopyTwo.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AviaSalesApiCopyTwo.Infrastructure.Middleware
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _nxt;

        public ExceptionHandlingMiddleware(RequestDelegate nxt)
        {
            _nxt = nxt;
        }
        
        public async Task Invoke(HttpContext ctx)
        {
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
            response.ContentType = "application/problem+json";
            response.StatusCode = (int) statusCode;
            await response.WriteAsync(JsonConvert.SerializeObject(new
            {
                StatusCode = (int) statusCode,
                Error = ex.Message
            }));
        }
    }
}