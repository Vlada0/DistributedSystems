using AviaSalesApiCopyTwo.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AviaSalesApiCopyTwo.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseExceptionHandlingMiddleware(this IApplicationBuilder app) => 
            app.UseMiddleware<ExceptionHandlingMiddleware>();

        public static void UseSwaggerUiMiddleware(this IApplicationBuilder app) =>
            app.UseSwaggerUI(s => 
                s.SwaggerEndpoint("/swagger/AviaSalesApiSpecs/swagger.json", "AviaSales API"));
        
        public static void UseClientExceptionPage(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async ctx =>
                {
                    ctx.Response.StatusCode = 500;
                    await ctx.Response.WriteAsync("An unexpected fault happened. Try again later.");
                });
            });
        }
    }
}