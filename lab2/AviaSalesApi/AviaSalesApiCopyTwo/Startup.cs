using System;
using AutoMapper;
using AviaSalesApiCopyTwo.Data.Repository.Impl;
using AviaSalesApiCopyTwo.Data.Repository.Interfaces;
using AviaSalesApiCopyTwo.Extensions;
using AviaSalesApiCopyTwo.Infrastructure.Config;
using AviaSalesApiCopyTwo.Services.Impl;
using AviaSalesApiCopyTwo.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace AviaSalesApiCopyTwo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .ConfigureControllers()
                .ConfigureSwagger()
                .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                .Configure<MongoDbConnectionSettings>(_configuration.GetSection(nameof(MongoDbConnectionSettings)))
                .AddSingleton<IMongoDbConnectionSettings>(sp => sp.GetRequiredService<IOptions<MongoDbConnectionSettings>>().Value)
                .AddSingleton(typeof(IMongoRepository<>), typeof(MongoRepository<>))
                .AddTransient<ITicketService, TicketService>()
                .AddTransient<IWarrantsService, WarrantService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseClientExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUiMiddleware();
            
            app.UseExceptionHandlingMiddleware();

            app.UseRouting();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}