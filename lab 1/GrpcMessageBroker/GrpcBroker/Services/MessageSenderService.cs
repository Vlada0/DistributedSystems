using Grpc.Core;
using GrpcAgent;
using GrpcBroker.DTO;
using GrpcBroker.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcBroker.Services
{
	public class MessageSenderService : IHostedService
	{
		const int Period = 1500;
		private readonly IConnectionManagementService _connectionService;
		private readonly IMessageStorageService _messageStorageService;
		private Timer _timer;

		public MessageSenderService(IServiceScopeFactory scopeFactory)
		{
			using var scope = scopeFactory.CreateScope();
			_connectionService = scope.ServiceProvider.GetRequiredService<IConnectionManagementService>();
			_messageStorageService = scope.ServiceProvider.GetRequiredService<IMessageStorageService>();
		}
		public Task StartAsync(CancellationToken cancellationToken)
		{
			_timer = new Timer(SendMessages, null, 0, Period);
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken) => Task.FromResult(_timer?.Change(Timeout.Infinite, 0));

		private async void SendMessages(object state)
		{
			while (!_messageStorageService.IsEmpty())
			{
				var msg = _messageStorageService.GetNext();
				if(msg != null)
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

				if(e.StatusCode == StatusCode.Internal)
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
