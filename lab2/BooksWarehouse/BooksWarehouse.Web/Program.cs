using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksWarehouse.Domain.Entities;
using BooksWarehouse.Infrastructure.Data;
using BooksWarehouse.Web.Seed;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BooksWarehouse.Web
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();
			using var scope = host.Services.CreateScope();
			try
			{
				var countryRepository = scope.ServiceProvider.GetService<IMongoDbRepository<Country>>();
				var authorsRepository = scope.ServiceProvider.GetService<IMongoDbRepository<Author>>();
				//await DataSeeder.SeedCountries(countryRepository);
				//await DataSeeder.SeedAuthors(authorsRepository, countryRepository);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			await host.RunAsync();
		}

		private static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
