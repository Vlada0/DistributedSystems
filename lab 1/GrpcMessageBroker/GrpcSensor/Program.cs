using Grpc.Net.Client;
using GrpcAgent;
using Shared;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GrpcSensor
{
	class Program
	{
		const int high = 4096;
		const int low = 2048;

		static async Task Main(string[] args)
		{
			Console.WriteLine("Running sensor...");

			var channel = CreateSecureChannel();
			var publisherClient = new Publisher.PublisherClient(channel);
			
			var rnd = new Random();

			Console.WriteLine("Sensor type: ");
			var sensor = Console.ReadLine().Trim().ToLower();

			while (true)
			{
				var data = rnd.Next(low, high + 1);
				var command = new PublishCommand { Sensor = sensor, Data = data };
				try
				{
					await publisherClient.PublishMessageAsync(command);
					Console.WriteLine($"Send: {sensor} -> {data}");
					await Task.Delay(data);
				}
				catch (Exception e)
				{
					Console.WriteLine($"Could not send data. {e.Message}");
				}
			}
		}

		private static GrpcChannel CreateSecureChannel()
		{
			var httpClientHandler = new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback =
				HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
			};
			var httpClient = new HttpClient(httpClientHandler);

			return GrpcChannel.ForAddress(Consts.BrokerAddress, new GrpcChannelOptions { HttpClient = httpClient });
		}
	}
}
