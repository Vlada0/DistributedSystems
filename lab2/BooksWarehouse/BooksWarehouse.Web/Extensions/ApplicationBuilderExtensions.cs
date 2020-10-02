using BooksWarehouse.Web.Middleware;
using Microsoft.AspNetCore.Builder;

namespace BooksWarehouse.Web.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseExceptionHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}