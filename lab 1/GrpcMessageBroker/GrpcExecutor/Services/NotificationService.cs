using Grpc.Core;
using GrpcAgent;
using GrpcExecutor.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrpcExecutor.Services
{
	public class NotificationService : Notifier.NotifierBase
	{
		private readonly DataStorageService _storage;
		static readonly Dictionary<ExecutorTypeEnum, string[]> Scenarios = new Dictionary<ExecutorTypeEnum, string[]>
		{
			[ExecutorTypeEnum.Valve] = new string[] { "Valve has opened.", "Valve has closed." },
			[ExecutorTypeEnum.Pump] = new string[] { "Air has pumped in.", "Air has pumped out." },
			[ExecutorTypeEnum.Switch] = new string[] { "Light has turned on.", "Light has turned off." }
		};

		static ExecutorTypeEnum executor = default;

        public NotificationService(DataStorageService storage) => _storage = storage;

		public override Task<NotifyResponse> Notify(NotifyCommand command, ServerCallContext context)
		{
			executor = (ExecutorTypeEnum)command.ExecutorType;
			var color = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;

			var min = _storage.GetMinLastValue();
			var max = _storage.GetMaxLastValue();

			if(command.Data > 3500 && min <= 3500)
            {
				OutputMessage(0, command);
			}
			else if(command.Data <= 3500 && max > 3500)
            {
				OutputMessage(1, command);
			}

			_storage.AddMax(command.Data);
			_storage.AddMin(command.Data);
			
			Console.ForegroundColor = color;
			
			return Task.FromResult(new NotifyResponse());
		}

		private static void OutputMessage(int index, NotifyCommand command) => 
			Console.WriteLine($"Receive: {command.Sensor} - {command.Data}: {Scenarios[executor][index]}");
	}
}
