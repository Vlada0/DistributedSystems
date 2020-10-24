using System;
using AutoMapper;
using AviaSalesApi.Data.Repository.Impl;
using AviaSalesApi.Data.Repository.Interfaces;
using AviaSalesApi.Extensions;
using AviaSalesApi.Helpers;
using AviaSalesApi.Infrastructure.Config;
using AviaSalesApi.Services.Impl;
using AviaSalesApi.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace AviaSalesApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .ConfigureControllers()
                .ConfigureSwagger()
                .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                .AddSingleton(_configuration.GetSection(nameof(SeedDataConfig)).Get<SeedDataConfig>())
                .Configure<MongoDbConnectionSettings>(_configuration.GetSection(nameof(MongoDbConnectionSettings)))
                .AddSingleton<IMongoDbConnectionSettings>(sp => sp.GetRequiredService<IOptions<MongoDbConnectionSettings>>().Value)
                .AddSingleton(typeof(IMongoRepository<>), typeof(MongoRepository<>))
                .AddSingleton<IJsonFileProcessor, JsonFileProcessor>()
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