using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AviaSalesApi.Data.Repository.Impl;
using AviaSalesApi.Data.Repository.Interfaces;
using AviaSalesApi.Extensions;
using AviaSalesApi.Helpers;
using AviaSalesApi.Infrastructure.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
                .Configure<CassandraDbConfig>(_configuration.GetSection(nameof(CassandraDbConfig)))
                .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                .AddSingleton<ICassandraDbConfig>(sp => sp.GetRequiredService<IOptions<CassandraDbConfig>>().Value)
                .AddSingleton<IJsonFileProcessor, JsonFileProcessor>()
                .AddScoped(typeof(ICassandraRepository<>), typeof(CassandraRepository<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseExceptionHandlingMiddleware();
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}