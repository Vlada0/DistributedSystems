using Grpc.Net.Client;
using GrpcAgent;
using System.Net.Http;

namespace GrpcBroker.DTO
{
	public class Connection
	{
		public string Address { get; private set; }
		public string Sensor { get; private set; }
		public int ExecutorType { get; private set; }
		public GrpcChannel GrpcChannel { get; private set; }

		public static Connection From(SubscribeRequest request) => 
			new Connection 
			{ 
				Address = request.Address,
				Sensor = request.Sensor,
				ExecutorType = request.ExecutorType,
				GrpcChannel = CreateSecureChannel(request.Address)
			};

		private static GrpcChannel CreateSecureChannel(string address)
		{
			var httpClientHandler = new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback =
							HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
			};

			var httpClient = new HttpClient(httpClientHandler);

			return GrpcChannel.ForAddress(address,
				new GrpcChannelOptions { HttpClient = httpClient });
		}
	}
}
