using AviaSalesApi.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;

namespace AviaSalesApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseExceptionHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}