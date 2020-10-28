using System;
using System.Threading.Tasks;
using AviaSalesApi.Data;
using AviaSalesApi.Data.Entities;
using AviaSalesApi.Data.Repository.Interfaces;
using AviaSalesApi.Helpers;
using AviaSalesApi.Infrastructure.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// ReSharper disable All

namespace AviaSalesApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            try
            {
                var seedDataCfg = scope.ServiceProvider.GetService<SeedDataConfig>();
                if (seedDataCfg.ReseedExampleData)
                {
                    var processor = scope.ServiceProvider.GetService<IJsonFileProcessor>();
                    var ticketsRepository = scope.ServiceProvider.GetService<IMongoRepository<Ticket>>();
                    await Seed.SeedToMongoAsync(ticketsRepository, processor);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Running api on http://localhost:5000");
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => 
                { 
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://localhost:5000");
                });
    }
}