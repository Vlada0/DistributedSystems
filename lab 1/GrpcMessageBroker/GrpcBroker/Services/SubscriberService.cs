using Grpc.Core;
using GrpcAgent;
using GrpcBroker.DTO;
using GrpcBroker.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace GrpcBroker.Services
{
	public class SubscriberService : Subscriber.SubscriberBase
	{
		private readonly IConnectionManagementService _connectionManagementService;
		private readonly IMessageStorageService _messageStorageService;

		public SubscriberService(
			IConnectionManagementService connectionManagementService,
			IMessageStorageService messageStorageService)
		{
			_connectionManagementService = connectionManagementService;
			_messageStorageService = messageStorageService;
		}

		public override Task<SubscribeResponse> Subscribe(SubscribeRequest request, ServerCallContext context)
		{
			try
			{
				var connection = Connection.From(request);
				var result = _connectionManagementService.TryAddConnection(connection);
				var exestingSensors = _messageStorageService.GetSensorsList();
				return Task.FromResult(new SubscribeResponse { IsSuccess = result, Sensors = string.Join(';', exestingSensors) });
			}
			catch (Exception e)
			{
				Console.WriteLine($"Could not add new subscriber with address {request.Address}. {e.Message}");
				return Task.FromResult(new SubscribeResponse { IsSuccess = false });
			}
		}
	}
}
