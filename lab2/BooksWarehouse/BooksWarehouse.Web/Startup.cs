using AutoMapper;
using BooksWarehouse.Infrastructure;
using BooksWarehouse.Infrastructure.Config;
using BooksWarehouse.Infrastructure.Data;
using BooksWarehouse.Infrastructure.Models;
using BooksWarehouse.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

// ReSharper disable MemberCanBePrivate.Global

namespace BooksWarehouse.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.ConfigureControllers();

			services
				.Configure<MongoDbConnectionSettings>(Configuration.GetSection(nameof(MongoDbConnectionSettings)))
				.AddSingleton<IMongoDbConnectionSettings>(sp =>
					sp.GetRequiredService<IOptions<MongoDbConnectionSettings>>().Value)
				.AddSingleton<IMediator, Mediator>()
				.AddSingleton(typeof(IMongoDbRepository<>), typeof(MongoDbMongoDbRepository<>))
				.AddCommandQueryHandlers(typeof(IQueryHandler<,>))
				.AddCommandQueryHandlers(typeof(ICommandHandler<>))
				.AddAutoMapper(typeof(AutomapperConfig).Assembly);
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
