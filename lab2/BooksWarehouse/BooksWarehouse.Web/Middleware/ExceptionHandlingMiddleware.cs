using System;
using System.Net;
using System.Threading.Tasks;
using BooksWarehouse.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
// ReSharper disable ClassNeverInstantiated.Global

namespace BooksWarehouse.Web.Middleware
{
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
            catch (BadRequestException e)
            {
                await HandleExceptionAsync(ctx, e, HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(ctx, e, HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleExceptionAsync(HttpContext ctx, Exception ex, HttpStatusCode statusCode)
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