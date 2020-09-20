using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcAgent;
using GrpcExecutor.Infrastructure;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Shared;

namespace GrpcExecutor
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var host = WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.UseUrls(Consts.ExecutorAddress)
				.Build();

			await host.StartAsync();

			Console.WriteLine("Executor started...");
			var executorTypes = Enum.GetValues(typeof(ExecutorTypeEnum)).Cast<ExecutorTypeEnum>();

			var builder = new StringBuilder();
			for(int i = 0; i < executorTypes.Count(); i++)
			{
				builder.Append($"{i + 1}. {executorTypes.ElementAt(i)} {(i < executorTypes.Count() - 1 ? " |" : string.Empty)}");
			}

			Console.WriteLine(builder);

			Console.WriteLine("Executor type: ");
			int.TryParse(Console.ReadLine().Trim(), out var input);

			await SubscribeAsync(host, input);

			Console.ReadLine();
		}

		private static async Task SubscribeAsync(IWebHost host, int executorType)
		{
			var address = GetHostAddress(host);
			Console.WriteLine($"Running executor on {address}");

			var sensorsListGetter = CreateSensorsListGetterClient();

			await OutputSensorsList(sensorsListGetter);

			var subscriberClient = CreateSubscriberClient();

			Console.WriteLine("Sensor type: ");
			var sensor = Console.ReadLine().Trim().ToLower();

			var request = new SubscribeRequest { Sensor = sensor, Address = address };

			try
			{
				var response = await subscriberClient.SubscribeAsync(request);
				if (!response.IsSuccess)
				{
					Console.WriteLine($"Could not subscribe to a sensor {sensor}");
				}
				else
				{
					Console.WriteLine("Subscription succeed.");
					Console.WriteLine("");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine($"Couldn not connect. {e.Message}");
			}
		}

		private static async Task OutputSensorsList(SensorsListGetter.SensorsListGetterClient sensorsListGetter)
		{
			try
			{
				var response = await sensorsListGetter.GetSensorsListAsync(new SensorsListRequest());
				if (!response.IsEmpty)
				{
					Console.WriteLine(response.Sensors);
				}
				else
				{
					Console.WriteLine("No sensors connected.");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine($"Could not get sensors list. {e.Message}");
			}
		}

		private static Subscriber.SubscriberClient CreateSubscriberClient()
		{
			var httpClientHandler = new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback =
							HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
			};

			var httpClient = new HttpClient(httpClientHandler);

			var channel = GrpcChannel.ForAddress(Consts.BrokerAddress, 
				new GrpcChannelOptions { HttpClient = httpClient });

			return new Subscriber.SubscriberClient(channel);
		}

		private static SensorsListGetter.SensorsListGetterClient CreateSensorsListGetterClient()
		{
			var httpClientHandler = new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback =
							HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
			};

			var httpClient = new HttpClient(httpClientHandler);
			var channel = GrpcChannel.ForAddress(Consts.BrokerAddress,
				new GrpcChannelOptions { HttpClient = httpClient });

			return new SensorsListGetter.SensorsListGetterClient(channel);
		}

		private static string GetHostAddress(IWebHost host) => 
			host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.FirstOrDefault();
	}
}
