using Grpc.Core;
using GrpcAgent;
using GrpcBroker.DTO;
using GrpcBroker.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace GrpcBroker.Services
{
    public class MessageSenderHandlerService : IMessageSenderHandlerService
    {
        private readonly IConnectionManagementService _connectionService;
        private readonly IMessageStorageService _messageStorageService;

        public MessageSenderHandlerService(IConnectionManagementService connectionService, IMessageStorageService messageStorageService)
        {
            _connectionService = connectionService;
            _messageStorageService = messageStorageService;
        }
        public async Task SendMessageAsync(Message message)
        {
			while (!_messageStorageService.IsEmpty())
			{
				var msg = _messageStorageService.GetNext();
				if (msg != null)
				{
					var connections = _connectionService.GetConnectionsBySensor(msg.Sensor);
					foreach (var conn in connections)
					{
						await NotifySubscriber(msg, conn);
					}
				}
			}
		}

		private async Task NotifySubscriber(Message msg, Connection conn)
		{
			try
			{
				var notifier = new Notifier.NotifierClient(conn.GrpcChannel);
				var command = new NotifyCommand { Sensor = msg.Sensor, Data = msg.Data, ExecutorType = conn.ExecutorType };
				await notifier.NotifyAsync(command);
			}
			catch (RpcException e)
			{
				Console.WriteLine($"Could not send to {conn.Address} | {e.Message}");

				if (e.StatusCode == StatusCode.Internal)
				{
					_connectionService.RemoveConnection(conn.Address);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine($"Could not send to {conn.Address} | {e.Message}");
			}
		}
	}
}
